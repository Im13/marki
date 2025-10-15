using API.DTOs;
using API.DTOs.Product;
using API.Helpers;
using AutoMapper;
using Core;
using Core.Entities;
using Core.Interfaces;
using Core.Specification;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Admin
{
    public class AdminProductController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IProductService _productService;
        private readonly IPhotoService _photoService;
        private readonly IGenericRepository<Product> _genericProductRepo;
        private readonly IProductRepository _productRepo;
        private readonly ILogger<AdminProductController> _logger;

        public AdminProductController(IProductService productService,
            IMapper mapper,
            IGenericRepository<Product> genericProductRepo,
            IProductRepository productRepo,
            IPhotoService photoService,
            ILogger<AdminProductController> logger)
        {
            _productRepo = productRepo;
            _productService = productService;
            _logger = logger;
            _mapper = mapper;
            _genericProductRepo = genericProductRepo;
            _photoService = photoService;
        }

        [HttpPost("create")]
        public async Task<ActionResult<ProductToReturnDTO>> CreateProducts(ProductDTOs productDTOs)
        {
            var product = _mapper.Map<ProductDTOs, Product>(productDTOs);

            var createdProduct = await _productService.CreateProduct(product);

            return Ok(createdProduct);
        }

        [HttpGet("products")]
        public async Task<ActionResult<Pagination<ProductDTOs>>> GetProducts([FromQuery] ProductSpecParams productParams)
        {
            var spec = new ProductsWithTypesSpecification(productParams);

            var countSpec = new ProductWithFiltersForCountSpecification(productParams);

            var totalItems = await _genericProductRepo.CountAsync(countSpec);

            var products = await _productRepo.GetProductsWithSpec(spec);

            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductDTOs>>(products);

            return Ok(new Pagination<ProductDTOs>(productParams.PageIndex, productParams.PageSize, totalItems, data));
        }

        [HttpPut("product/{id}")]
        public async Task<ActionResult> UpdateProduct(int id, [FromBody] UpdateProductDTO dto)
        {
            try
            {
                _logger.LogInformation("Updating product: ID={ProductId}", id);
                var product = _mapper.Map<UpdateProductDTO, Product>(dto);

                var result = await _productService.UpdateProductAsync(id, product);

                if (!result.IsSuccess)
                {
                    return BadRequest(new { message = result.Error });
                }

                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating product: ID={ProductId}", id);
                return StatusCode(500, new { message = "An error occurred while updating the product" });
            }
        }

        // Replace with SoftDeleteProducts
        [HttpPost("delete-products")]
        public async Task<ActionResult> DeleteProduct(List<ProductDTOs> productDTOs)
        {
            if (productDTOs.Count <= 0)
                return BadRequest("No data received!");

            var products = _mapper.Map<List<ProductDTOs>, List<Product>>(productDTOs);

            var deletedResult = await _productService.DeleteProducts(products);

            if (!deletedResult) return BadRequest("Failed to delete!");

            return Ok();
        }

        [HttpDelete("product/{id}")]
        public async Task<ActionResult> SoftDeleteProduct(int id)
        {
            var success = await _productService.SoftDeleteProductAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpDelete("products")]
        public async Task<ActionResult> SoftDeleteProducts([FromBody] IEnumerable<int> productIds)
        {
            if (productIds == null) return BadRequest("No data received!");
            var success = await _productService.SoftDeleteProductsAsync(productIds);
            if (!success) return BadRequest("Failed to delete!");
            return NoContent();
        }

        [HttpPost("image-upload")]
        public async Task<ActionResult<PhotoDTO>> UploadImage(IFormFile file)
        {
            var result = await _photoService.AddPhotoAsync(file);

            if (result.Error != null) return BadRequest(result.Error.Message);

            var photoDTO = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
                IsMain = false
            };

            return Ok(_mapper.Map<Photo, PhotoDTO>(photoDTO));
        }

        [HttpGet("sku/{id}")]
        public async Task<ActionResult<ProductSKUDetailDTO>> GetSku(int id)
        {
            var returnedSKU = await _productService.GetProductSKU(id);

            return Ok(_mapper.Map<ProductSKUDetailDTO>(returnedSKU));
        }

        [HttpGet("skus")]
        public async Task<ActionResult<ProductSKUDetailDTO[]>> GetAllSkus([FromQuery] ProductSpecParams productParams)
        {
            var productSkuList = new List<ProductSKUs>();

            var spec = new ProductsWithTypesSpecification(productParams);

            var products = await _productRepo.GetProductsWithSpec(spec);

            foreach (var product in products)
            {
                productSkuList.AddRange(product.ProductSKUs);
            }

            var productSkus = _mapper.Map<IReadOnlyList<ProductSKUs>, IReadOnlyList<ProductSKUDetailDTO>>(productSkuList);

            return Ok(productSkus);
        }
    }
}