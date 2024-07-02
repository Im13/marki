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
        private readonly IGenericRepository<Product> _genericProductRepo;
        private readonly IProductRepository _productRepo;

        public AdminProductController(IProductService productService, IMapper mapper, IGenericRepository<Product> genericProductRepo, IProductRepository productRepo)
        {
            _productRepo = productRepo;
            _productService = productService;
            _mapper = mapper;
            _genericProductRepo = genericProductRepo;
        }

        [HttpPost("create")]
        public async Task<ActionResult<ProductToReturnDTO>> CreateProducts(ProductDTOs productDTOs)
        {
            var options = _mapper.Map<List<ProductOptionDTO>, List<ProductOptions>>(productDTOs.ProductOptions);
            var product = _mapper.Map<ProductDTOs, Product>(productDTOs);

            var productWithSKUExists = await _productService.GetProductBySKUAsync(productDTOs.ProductSKU);

            if (productWithSKUExists != null) return BadRequest("Product with this SKU has exists!");

            var createdProduct = await _productService.CreateProduct(product, options);

            return Ok(createdProduct);
        }

        [HttpGet("products")]
        public async Task<ActionResult<Pagination<ProductDTO>>> GetProducts([FromQuery] ProductSpecParams productParams)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(productParams);

            var countSpec = new ProductWithFiltersForCountSpecification(productParams);

            var totalItems = await _genericProductRepo.CountAsync(countSpec);

            var products = await _productRepo.GetProductsWithSpec(spec);

            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductDTOs>>(products);

            return Ok(new Pagination<ProductDTOs>(productParams.PageIndex, productParams.PageSize, totalItems, data));
        }

        [HttpPut("product")]
        public async Task<ActionResult> UpdateProduct(ProductDTOs productDTO)
        {
            if(productDTO.Id == null) return BadRequest("Error update product!");

            // Find product by Id
            var product = await _productService.GetProductAsync((int)productDTO.Id);

            if (product == null) return BadRequest("Product not exists!");

            //Check if product with SKU exists
            if (product.ProductSKU != productDTO.ProductSKU)
            {
                var productWithSKUExists = await _productService.GetProductBySKUAsync(productDTO.ProductSKU);

                if (productWithSKUExists != null) return BadRequest("Product with this SKU has exists!");
            }

            var mappedProduct = _mapper.Map<ProductDTOs, Product>(productDTO);

            var productUpdatedResult = await _productService.UpdateProduct(mappedProduct);

            if (productUpdatedResult == null) return BadRequest("Error update product.");

            return Ok(productUpdatedResult);
        }

        [HttpDelete("products")]
        public async Task<ActionResult> DeleteProduct(List<int> productDTOs)
        {
            if(productDTOs.Count <= 0)
                return BadRequest("No data received!");

            // var products = _mapper.Map<List<ProductDTOs, Product>>    
        
            return Ok();
        }
    }
}