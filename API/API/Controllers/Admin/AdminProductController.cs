using API.DTOs;
using API.DTOs.Product;
using API.Helpers;
using AutoMapper;
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
    }
}