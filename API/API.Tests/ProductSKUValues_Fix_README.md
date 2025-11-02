# Product Creation Fix - ProductSKUValues Validation

## Vấn đề đã giải quyết

### Vấn đề gốc:
Khi tạo product mới thông qua API `/api/admin/create`, dữ liệu được lưu nhưng **ProductSKUValues bị rỗng**, dẫn đến:
- Frontend không thể tìm được SKU phù hợp khi user chọn options
- Lỗi "Không tìm thấy đối tượng nào thỏa mãn điều kiện" khi Add to Cart
- Dữ liệu không đầy đủ trong database

### Nguyên nhân:
1. Request body không có `ProductSKUValues` hoặc có nhưng không có `ValueTempId`
2. Backend không validate ProductSKUValues trước khi save
3. Không có kiểm tra sau khi save để đảm bảo data integrity

## Giải pháp đã triển khai

### 1. Enhanced Validation trong ProductRepository

#### Validation trước khi save:
```csharp
// Kiểm tra ProductSKUValues phải có khi có ProductOptions
if (product.ProductOptions != null && product.ProductOptions.Any())
{
    foreach (var sku in product.ProductSKUs)
    {
        if (sku.ProductSKUValues == null || !sku.ProductSKUValues.Any())
        {
            throw new ValidationException(
                $"SKU '{sku.SKU}' must have ProductSKUValues linking to ProductOptions");
        }
        
        // Kiểm tra số lượng values = số lượng options
        var expectedCount = product.ProductOptions.Count;
        var actualCount = sku.ProductSKUValues.Count;
        
        if (actualCount != expectedCount)
        {
            throw new ValidationException(
                $"SKU '{sku.SKU}' has {actualCount} values but product has {expectedCount} options");
        }
        
        // Kiểm tra ValueTempId hợp lệ
        var validValueTempIds = product.ProductOptions
            .SelectMany(o => o.ProductOptionValues)
            .Select(v => v.ValueTempId)
            .ToHashSet();
            
        foreach (var skuValue in sku.ProductSKUValues)
        {
            if (!validValueTempIds.Contains(skuValue.ValueTempId))
            {
                throw new ValidationException(
                    $"SKU '{sku.SKU}' has invalid ValueTempId {skuValue.ValueTempId}");
            }
        }
    }
}
```

#### Validation sau khi save:
```csharp
private async Task ValidateProductSKUValuesAfterSave(Product product)
{
    // Reload product với ProductSKUValues
    var verifyProduct = await _context.Products
        .Include(p => p.ProductSKUs)
            .ThenInclude(s => s.ProductSKUValues)
        .Include(p => p.ProductOptions)
        .FirstOrDefaultAsync(p => p.Id == product.Id);

    // Kiểm tra mỗi SKU có ProductSKUValues
    foreach (var sku in verifyProduct.ProductSKUs)
    {
        if (sku.ProductSKUValues == null || !sku.ProductSKUValues.Any())
        {
            throw new InvalidOperationException(
                $"SKU ID {sku.Id} has NO ProductSKUValues after save");
        }
    }
}
```

### 2. Comprehensive Unit Tests

Tạo 15+ test cases bao gồm:

#### ✅ Happy Path Tests:
- `CreateProduct_WithValidData_ShouldSucceed`
- `CreateProduct_WithProductOptions_ShouldCreateProductSKUValues`
- `CreateProduct_ShouldGenerateUniqueSlug`
- `CreateProduct_ShouldSetMainPhoto`

#### ❌ Error Case Tests:
- `CreateProduct_WithoutProductSKUValues_WhenOptionsExist_ShouldThrowValidationException`
- `CreateProduct_WithMismatchedValueCount_ShouldThrowValidationException`
- `CreateProduct_WithInvalidValueTempId_ShouldThrowValidationException`
- `CreateProduct_WithoutName_ShouldThrowValidationException`
- `CreateProduct_WithoutSKUs_ShouldThrowValidationException`
- `CreateProduct_WithDuplicateSKUCodes_ShouldThrowValidationException`
- `CreateProduct_WithInvalidPrice_ShouldThrowValidationException`

#### 🔄 Integration Tests:
- `CreateProduct_WithCompleteData_ShouldCreateProductWithSKUValues`
- `CreateProduct_WithoutProductSKUValues_ShouldReturnBadRequest`
- `CreateProduct_MultipleTimes_AllShouldHaveProductSKUValues`

## Cách sử dụng đúng

### Request Body mẫu khi tạo product:

```json
{
  "name": "Halter-neck knitted top",
  "description": "Beautiful knitted top",
  "productTypeId": 1,
  "productSKU": "KNIT-TOP-001",
  "importPrice": 50000,
  "productOptions": [
    {
      "optionName": "Size",
      "productOptionValues": [
        {
          "valueName": "S",
          "valueTempId": 1
        },
        {
          "valueName": "M",
          "valueTempId": 2
        },
        {
          "valueName": "L",
          "valueTempId": 3
        }
      ]
    },
    {
      "optionName": "Màu sắc",
      "productOptionValues": [
        {
          "valueName": "Đen",
          "valueTempId": 4
        },
        {
          "valueName": "Xanh lá",
          "valueTempId": 5
        },
        {
          "valueName": "Xanh dương",
          "valueTempId": 6
        }
      ]
    }
  ],
  "productSkus": [
    {
      "sku": "KNIT-TOP-001-S-DEN",
      "price": 150000,
      "quantity": 10,
      "barcode": "KT001SD",
      "weight": 200,
      "productSKUValues": [
        {
          "valueTempId": 1
        },
        {
          "valueTempId": 4
        }
      ]
    },
    {
      "sku": "KNIT-TOP-001-M-XANHLA",
      "price": 150000,
      "quantity": 15,
      "barcode": "KT001MXL",
      "weight": 220,
      "productSKUValues": [
        {
          "valueTempId": 2
        },
        {
          "valueTempId": 5
        }
      ]
    }
  ],
  "photos": [
    {
      "url": "https://example.com/photo1.jpg",
      "isMain": true
    }
  ]
}
```

### ⚠️ Quan trọng:

1. **Mỗi SKU phải có `productSKUValues`**
2. **Số lượng values phải = số lượng options** (Ví dụ: 2 options → mỗi SKU phải có 2 values)
3. **`valueTempId` phải khớp với ID trong `productOptionValues`**
4. **Mỗi combination của values phải unique** (không có 2 SKUs với cùng tổ hợp Size+Color)

## Chạy Tests

```bash
cd API/API.Tests
dotnet test
```

### Xem kết quả chi tiết:
```bash
dotnet test --logger "console;verbosity=detailed"
```

### Chạy specific test:
```bash
dotnet test --filter "FullyQualifiedName~ProductRepositoryTests"
```

## Debugging Tips

### Nếu vẫn gặp lỗi ProductSKUValues rỗng:

1. **Kiểm tra request body:**
   ```bash
   # Log request trong controller
   Console.WriteLine(JsonSerializer.Serialize(productDTOs));
   ```

2. **Kiểm tra AutoMapper:**
   ```csharp
   // Đảm bảo mapping ProductSKUValues
   CreateMap<ProductSKUDTO, ProductSKUs>()
       .ForMember(d => d.ProductSKUValues, o => o.MapFrom(s => s.ProductSKUValues));
   ```

3. **Kiểm tra database:**
   ```sql
   -- Xem ProductSKUValues
   SELECT ps.Id, ps.SKU, psv.Id as ValueId, pov.ValueName
   FROM ProductSKUs ps
   LEFT JOIN ProductSKUValues psv ON ps.Id = psv.ProductSKUId
   LEFT JOIN ProductOptionValues pov ON psv.ProductOptionValueId = pov.Id
   WHERE ps.ProductId = <your_product_id>
   ```

4. **Enable detailed logging:**
   ```csharp
   // appsettings.Development.json
   {
     "Logging": {
       "LogLevel": {
         "Infrastructure.Data.ProductRepository": "Debug"
       }
     }
   }
   ```

## Fix Data Tool

Nếu có products cũ bị thiếu ProductSKUValues, dùng endpoint:

```bash
POST /api/products/fix-sku-values/{productId}
```

Response:
```json
{
  "success": true,
  "fixedCount": 9,
  "totalSKUs": 9,
  "errors": [],
  "details": [
    {
      "skuId": 127,
      "skuCode": "KNIT-TOP-SDen",
      "size": "S",
      "color": "Đen",
      "success": true
    }
  ]
}
```

## Checklist khi tạo product mới

- [ ] Request có `productOptions` với `productOptionValues`
- [ ] Mỗi `productOptionValue` có `valueTempId` unique
- [ ] Mỗi `productSku` có `productSKUValues`
- [ ] Số lượng `productSKUValues` = số lượng `productOptions`
- [ ] Mỗi `valueTempId` trong `productSKUValues` tồn tại trong `productOptionValues`
- [ ] Không có duplicate SKU codes
- [ ] Tất cả prices > 0
- [ ] Đã test với unit tests
- [ ] Đã verify trong database sau khi tạo

## Liên hệ

Nếu gặp vấn đề, kiểm tra:
1. Logs trong console/file
2. Database integrity
3. Request body format
4. Run unit tests để tìm regression
