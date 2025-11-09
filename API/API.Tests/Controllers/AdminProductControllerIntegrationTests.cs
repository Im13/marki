// using API.Controllers.Admin;
// using API.DTOs.Product;
// using AutoMapper;
// using Core.Entities;
// using Core.Interfaces;
// using Infrastructure.Data;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.Logging;
// using Moq;
// using Xunit;

// namespace API.Tests.Controllers
// {
//     public class AdminProductControllerIntegrationTests : IDisposable
//     // {
//     //     private readonly StoreContext _context;
//     //     private readonly AdminProductController _controller;
//     //     private readonly IMapper _mapper;
//     //     private readonly Mock<IPhotoService> _photoServiceMock;
//     //     private readonly Mock<ILogger<AdminProductController>> _controllerLoggerMock;
//     //     private readonly Mock<ILogger<ProductRepository>> _repoLoggerMock;

//     //     public AdminProductControllerIntegrationTests()
//     //     {
//     //         // Setup in-memory database
//     //         var options = new DbContextOptionsBuilder<StoreContext>()
//     //             .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
//     //             .Options;

//     //         _context = new StoreContext(options);

//     //         // Seed ProductType
//     //         _context.ProductTypes.Add(new ProductType { Id = 1, Name = "T-Shirt" });
//     //         _context.SaveChanges();

//     //         // Setup AutoMapper
//     //         var config = new MapperConfiguration(cfg =>
//     //         {
//     //             cfg.CreateMap<ProductDTOs, Product>().ReverseMap();
//     //             cfg.CreateMap<ProductSKUDTO, ProductSKUs>().ReverseMap();
//     //             cfg.CreateMap<ProductSKUValuesDTO, ProductSKUValues>().ReverseMap();
//     //             cfg.CreateMap<ProductOptionDTO, ProductOptions>().ReverseMap();
//     //             cfg.CreateMap<ProductOptionValueDTO, ProductOptionValues>().ReverseMap();
//     //             cfg.CreateMap<PhotoDTO, Photo>().ReverseMap();
//     //         });
//     //         _mapper = config.CreateMapper();

//     //         // Setup mocks
//     //         _photoServiceMock = new Mock<IPhotoService>();
//     //         _controllerLoggerMock = new Mock<ILogger<AdminProductController>>();
//     //         _repoLoggerMock = new Mock<ILogger<ProductRepository>>();

//     //         // Setup services
//     //         var productRepo = new ProductRepository(_context, _repoLoggerMock.Object);
//     //         var productService = new Infrastructure.Services.ProductService(
//     //             new UnitOfWork(_context),
//     //             _context,
//     //             _photoServiceMock.Object,
//     //             productRepo,
//     //             _repoLoggerMock.Object
//     //         );

//     //         var genericProductRepo = new GenericRepository<Product>(_context);
//     //         var genericProductRepoMock = new Mock<IGenericRepository<Product>>();
//     //         genericProductRepoMock.Setup(x => x.CountAsync(It.IsAny<Core.Specification.ISpecification<Product>>()))
//     //             .ReturnsAsync(0);

//     //         _controller = new AdminProductController(
//     //             productService,
//     //             _mapper,
//     //             genericProductRepoMock.Object,
//     //             productRepo,
//     //             _photoServiceMock.Object,
//     //             _controllerLoggerMock.Object
//     //         );
//     //     }

//     //     public void Dispose()
//     //     {
//     //         _context.Database.EnsureDeleted();
//     //         _context.Dispose();
//     //     }

//     //     [Fact]
//     //     public async Task CreateProduct_WithCompleteData_ShouldCreateProductWithSKUValues()
//     //     {
//     //         // Arrange
//     //         var productDTO = CreateProductDTOWithOptions();

//     //         // Act
//     //         var result = await _controller.CreateProducts(productDTO);

//     //         // Assert
//     //         var okResult = Assert.IsType<OkObjectResult>(result.Result);
//     //         var createdProduct = Assert.IsType<Product>(okResult.Value);

//     //         // Verify in database
//     //         var savedProduct = await _context.Products
//     //             .Include(p => p.ProductSKUs)
//     //                 .ThenInclude(s => s.ProductSKUValues)
//     //                     .ThenInclude(v => v.ProductOptionValue)
//     //             .Include(p => p.ProductOptions)
//     //                 .ThenInclude(o => o.ProductOptionValues)
//     //             .FirstOrDefaultAsync(p => p.Id == createdProduct.Id);

//     //         Assert.NotNull(savedProduct);
//     //         Assert.Equal("Integration Test Product", savedProduct.Name);
//     //         Assert.Equal(2, savedProduct.ProductOptions.Count);
//     //         Assert.Equal(2, savedProduct.ProductSKUs.Count);

//     //         // CRITICAL: Verify ProductSKUValues were created
//     //         foreach (var sku in savedProduct.ProductSKUs)
//     //         {
//     //             Assert.NotNull(sku.ProductSKUValues);
//     //             Assert.Equal(2, sku.ProductSKUValues.Count); // Should have 2 values (Size + Color)
                
//     //             foreach (var skuValue in sku.ProductSKUValues)
//     //             {
//     //                 Assert.NotNull(skuValue.ProductOptionValue);
//     //                 Assert.NotEmpty(skuValue.ProductOptionValue.ValueName);
//     //             }
//     //         }
//     //     }

//     //     [Fact]
//     //     public async Task CreateProduct_WithoutProductSKUValues_ShouldReturnBadRequest()
//     //     {
//     //         // Arrange
//     //         var productDTO = CreateProductDTOWithOptions();
            
//     //         // Remove ProductSKUValues (simulating the bug)
//     //         foreach (var sku in productDTO.ProductSkus)
//     //         {
//     //             sku.ProductSKUValues = null;
//     //         }

//     //         // Act
//     //         var result = await _controller.CreateProducts(productDTO);

//     //         // Assert
//     //         var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
//     //         var errorMessage = badRequestResult.Value.ToString();
//     //         Assert.Contains("must have ProductSKUValues", errorMessage);
//     //     }

//     //     [Fact]
//     //     public async Task CreateProduct_WithMismatchedValueCount_ShouldReturnBadRequest()
//     //     {
//     //         // Arrange
//     //         var productDTO = CreateProductDTOWithOptions();
            
//     //         // Remove one value (should have 2, only has 1)
//     //         productDTO.ProductSkus.First().ProductSKUValues.RemoveAt(1);

//     //         // Act
//     //         var result = await _controller.CreateProducts(productDTO);

//     //         // Assert
//     //         var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
//     //         var errorMessage = badRequestResult.Value.ToString();
//     //         Assert.Contains("ProductSKUValues but product has", errorMessage);
//     //     }

//     //     [Fact]
//     //     public async Task CreateProduct_WithInvalidValueTempId_ShouldReturnBadRequest()
//     //     {
//     //         // Arrange
//     //         var productDTO = CreateProductDTOWithOptions();
            
//     //         // Set invalid ValueTempId
//     //         productDTO.ProductSkus.First().ProductSKUValues.First().ValueTempId = 9999;

//     //         // Act
//     //         var result = await _controller.CreateProducts(productDTO);

//     //         // Assert
//     //         var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
//     //         var errorMessage = badRequestResult.Value.ToString();
//     //         Assert.Contains("invalid ValueTempId", errorMessage);
//     //     }

//     //     [Fact]
//     //     public async Task CreateProduct_MultipleTimes_AllShouldHaveProductSKUValues()
//     //     {
//     //         // Arrange
//     //         var productsToCreate = 5;
//     //         var createdProducts = new List<Product>();

//     //         // Act
//     //         for (int i = 0; i < productsToCreate; i++)
//     //         {
//     //             var productDTO = CreateProductDTOWithOptions();
//     //             productDTO.Name = $"Product {i}";
//     //             productDTO.ProductSKU = $"TEST-{i:D3}";
                
//     //             foreach (var sku in productDTO.ProductSkus)
//     //             {
//     //                 sku.Sku = $"TEST-{i:D3}-{sku.Sku}";
//     //             }

//     //             var result = await _controller.CreateProducts(productDTO);
//     //             var okResult = Assert.IsType<OkObjectResult>(result.Result);
//     //             createdProducts.Add((Product)okResult.Value);
//     //         }

//     //         // Assert
//     //         foreach (var createdProduct in createdProducts)
//     //         {
//     //             var savedProduct = await _context.Products
//     //                 .Include(p => p.ProductSKUs)
//     //                     .ThenInclude(s => s.ProductSKUValues)
//     //                 .FirstOrDefaultAsync(p => p.Id == createdProduct.Id);

//     //             Assert.NotNull(savedProduct);
                
//     //             foreach (var sku in savedProduct.ProductSKUs)
//     //             {
//     //                 Assert.NotNull(sku.ProductSKUValues);
//     //                 Assert.NotEmpty(sku.ProductSKUValues);
//     //             }
//     //         }
//     //     }

//     //     private ProductDTOs CreateProductDTOWithOptions()
//     //     {
//     //         return new ProductDTOs
//     //         {
//     //             Name = "Integration Test Product",
//     //             Description = "Integration test description",
//     //             ProductTypeId = 1,
//     //             ProductSKU = "INT-TEST-001",
//     //             ImportPrice = 50,
//     //             ProductOptions = new List<ProductOptionDTO>
//     //             {
//     //                 new ProductOptionDTO
//     //                 {
//     //                     OptionName = "Size",
//     //                     ProductOptionValues = new List<ProductOptionValueDTO>
//     //                     {
//     //                         new ProductOptionValueDTO { ValueName = "S", ValueTempId = 1 },
//     //                         new ProductOptionValueDTO { ValueName = "M", ValueTempId = 2 }
//     //                     }
//     //                 },
//     //                 new ProductOptionDTO
//     //                 {
//     //                     OptionName = "Color",
//     //                     ProductOptionValues = new List<ProductOptionValueDTO>
//     //                     {
//     //                         new ProductOptionValueDTO { ValueName = "Red", ValueTempId = 3 },
//     //                         new ProductOptionValueDTO { ValueName = "Blue", ValueTempId = 4 }
//     //                     }
//     //                 }
//     //             },
//     //             ProductSkus = new List<ProductSKUDTO>
//     //             {
//     //                 new ProductSKUDTO
//     //                 {
//     //                     Sku = "S-RED",
//     //                     Price = 100,
//     //                     Quantity = 10,
//     //                     Barcode = "INT001",
//     //                     Weight = 100,
//     //                     ProductSKUValues = new List<ProductSKUValuesDTO>
//     //                     {
//     //                         new ProductSKUValuesDTO { ValueTempId = 1 }, // Size S
//     //                         new ProductSKUValuesDTO { ValueTempId = 3 }  // Color Red
//     //                     }
//     //                 },
//     //                 new ProductSKUDTO
//     //                 {
//     //                     Sku = "M-BLUE",
//     //                     Price = 110,
//     //                     Quantity = 15,
//     //                     Barcode = "INT002",
//     //                     Weight = 120,
//     //                     ProductSKUValues = new List<ProductSKUValuesDTO>
//     //                     {
//     //                         new ProductSKUValuesDTO { ValueTempId = 2 }, // Size M
//     //                         new ProductSKUValuesDTO { ValueTempId = 4 }  // Color Blue
//     //                     }
//     //                 }
//     //             }
//     //         };
//     //     }
//     // }
// }
