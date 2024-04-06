using API.DTOs;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specification;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AdminController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IProductService _productService;
        public AdminController(IGenericRepository<Product> productRepo, IMapper mapper, IProductService productService)
        {
            _productRepo = productRepo;
            _mapper = mapper;
            _productService = productService;
        }

        [HttpGet("products")]
        public async Task<ActionResult<Pagination<ProductDTO>>> GetProducts([FromQuery] ProductSpecParams productParams)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(productParams);

            var countSpec = new ProductWithFiltersForCountSpecification(productParams);

            var totalItems = await _productRepo.CountAsync(countSpec);

            var products = await _productRepo.ListAsync(spec);

            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductDTO>>(products);

            return Ok(new Pagination<ProductDTO>(productParams.PageIndex, productParams.PageSize, totalItems, data));
        }

        [HttpPut("product")]
        public async Task<ActionResult> EditProduct(ProductDTO productDTO)
        {
            //Find product by Id
            var product = await _productRepo.GetByIdAsync(productDTO.Id);

            if (product == null) return BadRequest("Product not exists!");

            //Check if product with SKU exists
            if (product.ProductSKU != productDTO.ProductSKU)
            {
                var productWithSKUExists = await _productService.GetProductBySKUAsync(productDTO.ProductSKU);

                if (productWithSKUExists != null) return BadRequest("Product with this SKU has exists!");
            }

            //Update product by productDTO
            product.ProductBrandId = productDTO.ProductBrandId;
            product.Price = productDTO.Price;
            product.ImportPrice = productDTO.ImportPrice;
            product.ProductTypeId = productDTO.ProductTypeId;
            product.Description = productDTO.Description;
            product.Name = productDTO.Name;
            product.ProductSKU = productDTO.ProductSKU;

            var productUpdatedResult = await _productService.UpdateProduct(product);

            if (productUpdatedResult == null) return BadRequest("Error update product.");

            return Ok(productUpdatedResult);
        }
    }
}