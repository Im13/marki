// using Core.Entities;
// using Infrastructure.Data;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.Logging;
// using Moq;
// using System.ComponentModel.DataAnnotations;
// using Xunit;

// namespace API.Tests.Repositories
// {
//     public class ProductRepositoryTests : IDisposable
//     {
//         private readonly StoreContext _context;
//         private readonly ProductRepository _repository;
//         private readonly Mock<ILogger<ProductRepository>> _loggerMock;

//         public ProductRepositoryTests()
//         {
//             // Setup in-memory database
//             var options = new DbContextOptionsBuilder<StoreContext>()
//                 .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
//                 .Options;

//             _context = new StoreContext(options);
//             _loggerMock = new Mock<ILogger<ProductRepository>>();
//             _repository = new ProductRepository(_context, _loggerMock.Object);

//             // Seed test data
//             SeedTestData();
//         }

//         private void SeedTestData()
//         {
//             // Add ProductType
//             _context.ProductTypes.Add(new ProductType { Id = 1, Name = "T-Shirt" });
//             _context.SaveChanges();
//         }

//         public void Dispose()
//         {
//             _context.Database.EnsureDeleted();
//             _context.Dispose();
//         }

//         #region CreateProduct Tests

//         [Fact]
//         public async Task CreateProduct_WithValidData_ShouldSucceed()
//         {
//             // Arrange
//             var product = CreateValidProduct();

//             // Act
//             var result = await _repository.CreateProductAsync(product);

//             // Assert
//             Assert.NotNull(result);
//             Assert.True(result.Id > 0);
//             Assert.Equal("Test Product", result.Name);
//             Assert.NotNull(result.Slug);
            
//             // Verify product was saved
//             var savedProduct = await _context.Products
//                 .Include(p => p.ProductSKUs)
//                     .ThenInclude(s => s.ProductSKUValues)
//                 .FirstOrDefaultAsync(p => p.Id == result.Id);
            
//             Assert.NotNull(savedProduct);
//             Assert.Equal(2, savedProduct.ProductSKUs.Count);
//         }

//         [Fact]
//         public async Task CreateProduct_WithProductOptions_ShouldCreateProductSKUValues()
//         {
//             // Arrange
//             var product = CreateProductWithOptions();

//             // Act
//             var result = await _repository.CreateProductAsync(product);

//             // Assert
//             var savedProduct = await _context.Products
//                 .Include(p => p.ProductSKUs)
//                     .ThenInclude(s => s.ProductSKUValues)
//                         .ThenInclude(v => v.ProductOptionValue)
//                 .Include(p => p.ProductOptions)
//                     .ThenInclude(o => o.ProductOptionValues)
//                 .FirstOrDefaultAsync(p => p.Id == result.Id);

//             Assert.NotNull(savedProduct);
            
//             // Verify ProductOptions were created
//             Assert.Equal(2, savedProduct.ProductOptions.Count);
//             var sizeOption = savedProduct.ProductOptions.First(o => o.OptionName == "Size");
//             Assert.Equal(2, sizeOption.ProductOptionValues.Count);

//             // Verify ProductSKUValues were created and linked
//             foreach (var sku in savedProduct.ProductSKUs)
//             {
//                 Assert.NotNull(sku.ProductSKUValues);
//                 Assert.Equal(2, sku.ProductSKUValues.Count); // 2 options = 2 values per SKU
                
//                 foreach (var skuValue in sku.ProductSKUValues)
//                 {
//                     Assert.NotNull(skuValue.ProductOptionValue);
//                     Assert.NotNull(skuValue.ProductOptionValue.ValueName);
//                 }
//             }
//         }

//         [Fact]
//         public async Task CreateProduct_WithoutProductSKUValues_WhenOptionsExist_ShouldThrowValidationException()
//         {
//             // Arrange
//             var product = CreateProductWithOptions();
            
//             // Remove ProductSKUValues from SKUs (simulating the bug)
//             foreach (var sku in product.ProductSKUs)
//             {
//                 sku.ProductSKUValues = null;
//             }

//             // Act & Assert
//             var exception = await Assert.ThrowsAsync<ValidationException>(
//                 () => _repository.CreateProductAsync(product)
//             );

//             Assert.Contains("must have ProductSKUValues", exception.Message);
//         }

//         [Fact]
//         public async Task CreateProduct_WithMismatchedValueCount_ShouldThrowValidationException()
//         {
//             // Arrange
//             var product = CreateProductWithOptions();
            
//             // Remove one ProductSKUValue (should have 2, but only has 1)
//             product.ProductSKUs.First().ProductSKUValues.RemoveAt(1);

//             // Act & Assert
//             var exception = await Assert.ThrowsAsync<ValidationException>(
//                 () => _repository.CreateProductAsync(product)
//             );

//             Assert.Contains("has 1 ProductSKUValues but product has 2 ProductOptions", exception.Message);
//         }

//         [Fact]
//         public async Task CreateProduct_WithInvalidValueTempId_ShouldThrowValidationException()
//         {
//             // Arrange
//             var product = CreateProductWithOptions();
            
//             // Set invalid ValueTempId that doesn't exist in ProductOptions
//             product.ProductSKUs.First().ProductSKUValues.First().ValueTempId = 9999;

//             // Act & Assert
//             var exception = await Assert.ThrowsAsync<ValidationException>(
//                 () => _repository.CreateProductAsync(product)
//             );

//             Assert.Contains("invalid ValueTempId 9999", exception.Message);
//         }

//         [Fact]
//         public async Task CreateProduct_WithoutName_ShouldThrowValidationException()
//         {
//             // Arrange
//             var product = CreateValidProduct();
//             product.Name = null;

//             // Act & Assert
//             var exception = await Assert.ThrowsAsync<ValidationException>(
//                 () => _repository.CreateProductAsync(product)
//             );

//             Assert.Contains("Product name is required", exception.Message);
//         }

//         [Fact]
//         public async Task CreateProduct_WithoutSKUs_ShouldThrowValidationException()
//         {
//             // Arrange
//             var product = CreateValidProduct();
//             product.ProductSKUs = null;

//             // Act & Assert
//             var exception = await Assert.ThrowsAsync<ValidationException>(
//                 () => _repository.CreateProductAsync(product)
//             );

//             Assert.Contains("At least one SKU is required", exception.Message);
//         }

//         [Fact]
//         public async Task CreateProduct_WithDuplicateSKUCodes_ShouldThrowValidationException()
//         {
//             // Arrange
//             var product = CreateValidProduct();
//             product.ProductSKUs.Add(new ProductSKUs
//             {
//                 SKU = product.ProductSKUs.First().SKU, // Duplicate SKU code
//                 Price = 100,
//                 Quantity = 10
//             });

//             // Act & Assert
//             var exception = await Assert.ThrowsAsync<ValidationException>(
//                 () => _repository.CreateProductAsync(product)
//             );

//             Assert.Contains("Duplicate SKU codes", exception.Message);
//         }

//         [Fact]
//         public async Task CreateProduct_WithInvalidPrice_ShouldThrowValidationException()
//         {
//             // Arrange
//             var product = CreateValidProduct();
//             product.ProductSKUs.First().Price = 0;

//             // Act & Assert
//             var exception = await Assert.ThrowsAsync<ValidationException>(
//                 () => _repository.CreateProductAsync(product)
//             );

//             Assert.Contains("All SKU prices must be greater than 0", exception.Message);
//         }

//         [Fact]
//         public async Task CreateProduct_ShouldGenerateUniqueSlug()
//         {
//             // Arrange
//             var product1 = CreateValidProduct();
//             product1.Name = "Test Product";
            
//             var product2 = CreateValidProduct();
//             product2.Name = "Test Product"; // Same name
//             product2.ProductSKU = "TEST-002";

//             // Act
//             var result1 = await _repository.CreateProductAsync(product1);
//             var result2 = await _repository.CreateProductAsync(product2);

//             // Assert
//             Assert.NotEqual(result1.Slug, result2.Slug);
//         }

//         [Fact]
//         public async Task CreateProduct_ShouldSetMainPhoto()
//         {
//             // Arrange
//             var product = CreateValidProduct();
//             product.Photos = new List<Photo>
//             {
//                 new Photo { Url = "photo1.jpg", IsMain = false },
//                 new Photo { Url = "photo2.jpg", IsMain = false }
//             };

//             // Act
//             var result = await _repository.CreateProductAsync(product);

//             // Assert
//             var savedProduct = await _context.Products
//                 .Include(p => p.Photos)
//                 .FirstOrDefaultAsync(p => p.Id == result.Id);

//             Assert.NotNull(savedProduct.Photos);
//             Assert.Single(savedProduct.Photos.Where(p => p.IsMain));
//         }

//         #endregion

//         #region UpdateProduct Tests

//         [Fact]
//         public async Task UpdateProduct_WithValidData_ShouldSucceed()
//         {
//             // Arrange
//             var product = await _repository.CreateProductAsync(CreateProductWithOptions());
            
//             var updatedProduct = CreateProductWithOptions();
//             updatedProduct.Name = "Updated Product Name";

//             // Act
//             var result = await _repository.UpdateProductAsync(product.Id, updatedProduct);

//             // Assert
//             Assert.Equal("Updated Product Name", result.Name);
//             Assert.NotEqual(product.Slug, result.Slug); // Slug should change
//         }

//         [Fact]
//         public async Task UpdateProduct_ShouldPreserveProductSKUValues()
//         {
//             // Arrange
//             var product = await _repository.CreateProductAsync(CreateProductWithOptions());
            
//             var updatedProduct = CreateProductWithOptions();
//             updatedProduct.Name = "Updated Product";

//             // Act
//             var result = await _repository.UpdateProductAsync(product.Id, updatedProduct);

//             // Assert
//             var savedProduct = await _context.Products
//                 .Include(p => p.ProductSKUs)
//                     .ThenInclude(s => s.ProductSKUValues)
//                 .FirstOrDefaultAsync(p => p.Id == result.Id);

//             foreach (var sku in savedProduct.ProductSKUs)
//             {
//                 Assert.NotNull(sku.ProductSKUValues);
//                 Assert.NotEmpty(sku.ProductSKUValues);
//             }
//         }

//         [Fact]
//         public async Task UpdateProduct_WithNonExistentId_ShouldThrowArgumentException()
//         {
//             // Arrange
//             var product = CreateValidProduct();

//             // Act & Assert
//             await Assert.ThrowsAsync<ArgumentException>(
//                 () => _repository.UpdateProductAsync(9999, product)
//             );
//         }

//         #endregion

//         #region Helper Methods

//         private Product CreateValidProduct()
//         {
//             return new Product
//             {
//                 Name = "Test Product",
//                 Description = "Test Description",
//                 ProductTypeId = 1,
//                 ProductSKU = "TEST-001",
//                 ImportPrice = 50,
//                 ProductSKUs = new List<ProductSKUs>
//                 {
//                     new ProductSKUs
//                     {
//                         SKU = "TEST-001-S",
//                         Price = 100,
//                         Quantity = 10,
//                         Barcode = "123456",
//                         Weight = 100
//                     },
//                     new ProductSKUs
//                     {
//                         SKU = "TEST-001-M",
//                         Price = 110,
//                         Quantity = 15,
//                         Barcode = "123457",
//                         Weight = 120
//                     }
//                 }
//             };
//         }

//         private Product CreateProductWithOptions()
//         {
//             var product = new Product
//             {
//                 Name = "Test Product With Options",
//                 Description = "Test Description",
//                 ProductTypeId = 1,
//                 ProductSKU = "TEST-002",
//                 ImportPrice = 50,
//                 ProductOptions = new List<ProductOptions>
//                 {
//                     new ProductOptions
//                     {
//                         OptionName = "Size",
//                         ProductOptionValues = new List<ProductOptionValues>
//                         {
//                             new ProductOptionValues { ValueName = "S", ValueTempId = 1 },
//                             new ProductOptionValues { ValueName = "M", ValueTempId = 2 }
//                         }
//                     },
//                     new ProductOptions
//                     {
//                         OptionName = "Color",
//                         ProductOptionValues = new List<ProductOptionValues>
//                         {
//                             new ProductOptionValues { ValueName = "Red", ValueTempId = 3 },
//                             new ProductOptionValues { ValueName = "Blue", ValueTempId = 4 }
//                         }
//                     }
//                 },
//                 ProductSKUs = new List<ProductSKUs>
//                 {
//                     new ProductSKUs
//                     {
//                         SKU = "TEST-002-S-RED",
//                         Price = 100,
//                         Quantity = 10,
//                         Barcode = "200001",
//                         Weight = 100,
//                         ProductSKUValues = new List<ProductSKUValues>
//                         {
//                             new ProductSKUValues { ValueTempId = 1 }, // Size S
//                             new ProductSKUValues { ValueTempId = 3 }  // Color Red
//                         }
//                     },
//                     new ProductSKUs
//                     {
//                         SKU = "TEST-002-M-BLUE",
//                         Price = 110,
//                         Quantity = 15,
//                         Barcode = "200002",
//                         Weight = 120,
//                         ProductSKUValues = new List<ProductSKUValues>
//                         {
//                             new ProductSKUValues { ValueTempId = 2 }, // Size M
//                             new ProductSKUValues { ValueTempId = 4 }  // Color Blue
//                         }
//                     }
//                 }
//             };

//             return product;
//         }

//         #endregion
//     }
// }
