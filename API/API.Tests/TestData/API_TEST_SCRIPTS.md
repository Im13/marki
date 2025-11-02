# Product Creation API Test Scripts

## Prerequisites
- Backend API running on http://localhost:5000
- ProductType with ID=1 exists in database

## Test 1: Valid Product (Should SUCCESS)
```bash
curl -X POST http://localhost:5000/api/admin/create \
  -H "Content-Type: application/json" \
  -d @valid-product-request.json
```

Expected Response:
```json
{
  "id": <generated_id>,
  "name": "Test Product - Complete Data",
  "productSKUs": [ ... ],
  ...
}
```

## Test 2: Missing ProductSKUValues (Should FAIL)
```bash
curl -X POST http://localhost:5000/api/admin/create \
  -H "Content-Type: application/json" \
  -d @invalid-missing-skuvalues.json
```

Expected Error:
```json
{
  "message": "SKU 'TEST-INVALID-001-S-DEN' must have ProductSKUValues linking to ProductOptions"
}
```

## Test 3: Mismatched Value Count (Should FAIL)
```bash
curl -X POST http://localhost:5000/api/admin/create \
  -H "Content-Type: application/json" \
  -d @invalid-mismatched-count.json
```

Expected Error:
```json
{
  "message": "SKU 'TEST-INVALID-002-S-DEN' has 1 ProductSKUValues but product has 2 ProductOptions"
}
```

## Test 4: Invalid ValueTempId (Should FAIL)
```bash
curl -X POST http://localhost:5000/api/admin/create \
  -H "Content-Type: application/json" \
  -d @invalid-wrong-valuetempid.json
```

Expected Error:
```json
{
  "message": "SKU 'TEST-INVALID-003-S-DEN' has invalid ValueTempId 9999"
}
```

## Verify in Database

After successful creation, verify ProductSKUValues:

```sql
-- Check product
SELECT * FROM Products WHERE ProductSKU = 'TEST-COMPLETE-001';

-- Check SKUs
SELECT * FROM ProductSKUs WHERE ProductId = <product_id>;

-- Check ProductSKUValues (MUST NOT BE EMPTY!)
SELECT 
    ps.Id as SKU_Id,
    ps.SKU as SKU_Code,
    psv.Id as Value_Id,
    pov.Id as OptionValue_Id,
    pov.ValueName as Value_Name,
    po.OptionName as Option_Name
FROM ProductSKUs ps
LEFT JOIN ProductSKUValues psv ON ps.Id = psv.ProductSKUId
LEFT JOIN ProductOptionValues pov ON psv.ProductOptionValueId = pov.Id
LEFT JOIN ProductOptions po ON pov.ProductOptionId = po.Id
WHERE ps.ProductId = <product_id>
ORDER BY ps.SKU, po.OptionName;
```

Expected result: Each SKU should have 2 rows (Size + Color values)

## Debug Endpoint

Check raw data:
```bash
curl http://localhost:5000/api/products/debug-slug/test-product-complete-data
```

## Postman Collection

Import this into Postman:

1. Create new Collection "Product Creation Tests"
2. Add Request "Create Valid Product"
   - Method: POST
   - URL: {{baseUrl}}/api/admin/create
   - Body: Raw JSON (copy from valid-product-request.json)
   
3. Add Request "Create Invalid - Missing SKUValues"
   - Method: POST
   - URL: {{baseUrl}}/api/admin/create
   - Body: Raw JSON (copy from invalid-missing-skuvalues.json)
   - Tests: 
     ```javascript
     pm.test("Should return 400 Bad Request", function () {
         pm.response.to.have.status(400);
     });
     
     pm.test("Error message mentions ProductSKUValues", function () {
         var jsonData = pm.response.json();
         pm.expect(JSON.stringify(jsonData)).to.include("ProductSKUValues");
     });
     ```

4. Add Request "Create Invalid - Mismatched Count"
   - Similar setup with invalid-mismatched-count.json

5. Add Request "Create Invalid - Wrong ValueTempId"
   - Similar setup with invalid-wrong-valuetempid.json

## Environment Variables

Set in Postman:
```
baseUrl = http://localhost:5000
```

## Run All Tests

Postman:
```
Collection Runner -> Select "Product Creation Tests" -> Run
```

Command line (requires newman):
```bash
npm install -g newman
newman run Product_Creation_Tests.postman_collection.json
```
