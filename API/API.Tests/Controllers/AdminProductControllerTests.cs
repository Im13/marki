// using API.Controllers.Admin;
// using API.DTOs;
// using API.DTOs.Product;
// using API.Helpers;
// using AutoMapper;
// using Core.Entities;
// using Core.Common;
// using Core;
// using Core.Interfaces;
// using Core.Specification;
// using Microsoft.AspNetCore.Http;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.Extensions.Logging;
// using Moq;

// namespace API.Tests.Controllers
// {
//     public class AdminProductControllerTests
//     {
//         private readonly Mock<IMapper> _mapper;
//         private readonly Mock<IProductService> _productService;
//         private readonly Mock<IPhotoService> _photoService;
//         private readonly Mock<IGenericRepository<Product>> _genericProductRepo;
//         private readonly Mock<IProductRepository> _productRepo;
//         private readonly Mock<ILogger<AdminProductController>> _logger;
//         private readonly AdminProductController _controller;

//         public AdminProductControllerTests()
//         {
//             _mapper = new Mock<IMapper>();
//             _productService = new Mock<IProductService>();
//             _photoService = new Mock<IPhotoService>();
//             _genericProductRepo = new Mock<IGenericRepository<Product>>();
//             _productRepo = new Mock<IProductRepository>();
//             _logger = new Mock<ILogger<AdminProductController>>();

//             _controller = new AdminProductController(
//                 _productService.Object,
//                 _mapper.Object,
//                 _genericProductRepo.Object,
//                 _productRepo.Object,
//                 _photoService.Object,
//                 _logger.Object
//             );
//         }

//         [Fact]
//         public async Task CreateProducts_ReturnsOk()
//         {
//             var dto = new ProductDTOs { Name = "Test" };
//             var entity = new Product { Name = "Test" };
//             _mapper.Setup(m => m.Map<ProductDTOs, Product>(dto)).Returns(entity);
//             _productService.Setup(s => s.CreateProduct(entity)).ReturnsAsync(entity);

//             var result = await _controller.CreateProducts(dto);

//             var ok = Assert.IsType<OkObjectResult>(result);
//             Assert.Equal(entity, ok.Value);
//         }

//         [Fact]
//         public async Task GetProducts_ReturnsPagedOk()
//         {
//             var p = new ProductSpecParams { PageIndex = 1, PageSize = 10 };
//             var products = new List<Product> { new Product { Id = 1, Name = "P1" } };
//             var mapped = new List<ProductDTOs> { new ProductDTOs { Name = "P1" } } as IReadOnlyList<ProductDTOs>;

//             _genericProductRepo.Setup(r => r.CountAsync(It.IsAny<ISpecification<Product>>())).ReturnsAsync(1);
//             _productRepo.Setup(r => r.GetProductsWithSpec(It.IsAny<ISpecification<Product>>()))
//                 .ReturnsAsync(products);
//             _mapper.Setup(m => m.Map<IReadOnlyList<Product>, IReadOnlyList<ProductDTOs>>(products)).Returns(mapped);

//             var result = await _controller.GetProducts(p);

//             var ok = Assert.IsType<OkObjectResult>(result.Result);
//             var payload = Assert.IsType<Pagination<ProductDTOs>>(ok.Value);
//             Assert.Equal(1, payload.Count);
//             Assert.Single(payload.Data);
//         }

//         [Fact]
//         public async Task UpdateProduct_OnSuccess_ReturnsOk()
//         {
//             var id = 5;
//             var dto = new UpdateProductDTO { Name = "New" };
//             var entity = new Product { Id = id, Name = "New" };
//             _mapper.Setup(m => m.Map<UpdateProductDTO, Product>(dto)).Returns(entity);
//             _productService.Setup(s => s.UpdateProductAsync(id, entity))
//                 .ReturnsAsync(Result<Product>.Success(entity));

//             var result = await _controller.UpdateProduct(id, dto);

//             var ok = Assert.IsType<OkObjectResult>(result);
//             Assert.Equal(entity, ok.Value);
//         }

//         [Fact]
//         public async Task UpdateProduct_OnFailure_ReturnsBadRequest()
//         {
//             var id = 5;
//             var dto = new UpdateProductDTO { Name = "New" };
//             var entity = new Product { Id = id, Name = "New" };
//             _mapper.Setup(m => m.Map<UpdateProductDTO, Product>(dto)).Returns(entity);
//             _productService.Setup(s => s.UpdateProductAsync(id, entity))
//                 .ReturnsAsync(Result<Product>.Failure("err"));

//             var result = await _controller.UpdateProduct(id, dto);

//             Assert.IsType<BadRequestObjectResult>(result);
//         }

//         [Fact]
//         public async Task DeleteProduct_Bulk_ReturnsOk_OnSuccess()
//         {
//             var listDto = new List<ProductDTOs> { new ProductDTOs { Id = 1 }, new ProductDTOs { Id = 2 } };
//             var listEntity = new List<Product> { new Product { Id = 1 }, new Product { Id = 2 } };
//             _mapper.Setup(m => m.Map<List<ProductDTOs>, List<Product>>(listDto)).Returns(listEntity);
//             _productService.Setup(s => s.DeleteProducts(listEntity)).ReturnsAsync(true);

//             var result = await _controller.DeleteProduct(listDto);

//             Assert.IsType<OkResult>(result);
//         }

//         [Fact]
//         public async Task DeleteProduct_Bulk_ReturnsBadRequest_OnFailure()
//         {
//             var listDto = new List<ProductDTOs> { new ProductDTOs { Id = 1 } };
//             var listEntity = new List<Product> { new Product { Id = 1 } };
//             _mapper.Setup(m => m.Map<List<ProductDTOs>, List<Product>>(listDto)).Returns(listEntity);
//             _productService.Setup(s => s.DeleteProducts(listEntity)).ReturnsAsync(false);

//             var result = await _controller.DeleteProduct(listDto);

//             var bad = Assert.IsType<BadRequestObjectResult>(result);
//             Assert.Equal("Failed to delete!", bad.Value);
//         }

//         [Fact]
//         public async Task SoftDeleteProduct_Returns204_WhenFound()
//         {
//             _productService.Setup(s => s.SoftDeleteProductAsync(10)).ReturnsAsync(true);

//             var result = await _controller.SoftDeleteProduct(10);

//             Assert.IsType<NoContentResult>(result);
//         }

//         [Fact]
//         public async Task SoftDeleteProduct_Returns404_WhenNotFound()
//         {
//             _productService.Setup(s => s.SoftDeleteProductAsync(10)).ReturnsAsync(false);

//             var result = await _controller.SoftDeleteProduct(10);

//             Assert.IsType<NotFoundResult>(result);
//         }

//         [Fact]
//         public async Task SoftDeleteProducts_Returns204_OnSuccess()
//         {
//             var ids = new List<int> { 1, 2, 3 };
//             _productService.Setup(s => s.SoftDeleteProductsAsync(ids)).ReturnsAsync(true);

//             var result = await _controller.SoftDeleteProducts(ids);

//             Assert.IsType<NoContentResult>(result);
//         }

//         [Fact]
//         public async Task SoftDeleteProducts_Returns400_OnFailure()
//         {
//             var ids = new List<int> { 1 };
//             _productService.Setup(s => s.SoftDeleteProductsAsync(ids)).ReturnsAsync(false);

//             var result = await _controller.SoftDeleteProducts(ids);

//             var bad = Assert.IsType<BadRequestObjectResult>(result);
//             Assert.Equal("Failed to delete!", bad.Value);
//         }

//         [Fact]
//         public async Task UploadImage_ReturnsOk_OnSuccess()
//         {
//             var file = new Mock<IFormFile>();
//             var uploadResult = new CloudinaryDotNet.Actions.ImageUploadResult
//             {
//                 SecureUrl = new System.Uri("https://example.com/img.jpg"),
//                 PublicId = "pid"
//             };
//             _photoService.Setup(s => s.AddPhotoAsync(It.IsAny<IFormFile>())).ReturnsAsync(uploadResult);
//             _mapper.Setup(m => m.Map<Photo, PhotoDTO>(It.IsAny<Photo>()))
//                 .Returns(new PhotoDTO { Url = uploadResult.SecureUrl.AbsoluteUri, PublicId = uploadResult.PublicId });

//             var result = await _controller.UploadImage(file.Object);

//             var ok = Assert.IsType<OkObjectResult>(result.Result);
//             var dto = Assert.IsType<PhotoDTO>(ok.Value);
//             Assert.Equal("https://example.com/img.jpg", dto.Url);
//         }

//         [Fact]
//         public async Task GetSku_ReturnsOk()
//         {
//             var detail = new ProductSKUs { Id = 1 };
//             _productService.Setup(s => s.GetProductSKU(1)).ReturnsAsync(detail);
//             _mapper.Setup(m => m.Map<ProductSKUDetailDTO>(detail))
//                 .Returns(new ProductSKUDetailDTO { Id = 1 });

//             var result = await _controller.GetSku(1);

//             var ok = Assert.IsType<OkObjectResult>(result.Result);
//             var dto = Assert.IsType<ProductSKUDetailDTO>(ok.Value);
//             Assert.Equal(1, dto.Id);
//         }

//         [Fact]
//         public async Task GetAllSkus_ReturnsOk()
//         {
//             var p = new ProductSpecParams();
//             var products = new List<Product>
//             {
//                 new Product { ProductSKUs = new List<ProductSKUs>{ new ProductSKUs { Id = 1 } }},
//                 new Product { ProductSKUs = new List<ProductSKUs>{ new ProductSKUs { Id = 2 } }}
//             };

//             _productRepo.Setup(r => r.GetProductsWithSpec(It.IsAny<ISpecification<Product>>()))
//                 .ReturnsAsync(products);
//             _mapper.Setup(m => m.Map<IReadOnlyList<ProductSKUs>, IReadOnlyList<ProductSKUDetailDTO>>(
//                     It.IsAny<IReadOnlyList<ProductSKUs>>()))
//                 .Returns(new List<ProductSKUDetailDTO> { new ProductSKUDetailDTO { Id = 1 }, new ProductSKUDetailDTO { Id = 2 } });

//             var result = await _controller.GetAllSkus(p);

//             var ok = Assert.IsType<OkObjectResult>(result.Result);
//             var list = Assert.IsType<List<ProductSKUDetailDTO>>(ok.Value);
//             Assert.Equal(2, list.Count);
//         }
//     }
// }


