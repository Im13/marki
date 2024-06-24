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
        public async Task<ActionResult> EditProduct(ProductDTOs productDTO)
        {
            var product = new Product();
            // int productId = 0;
            
            // if(productDTO.Id != null) 
            //     productId = product.Id;
            // else 
            //     return BadRequest("Error update product");

            // //Find product by Id
            // product = await _genericProductRepo.GetByIdAsync(productId);

            // if (product == null) return BadRequest("Product not exists!");

            // //Check if product with SKU exists
            // if (product.ProductSKU != productDTO.ProductSKU)
            // {
            //     var productWithSKUExists = await _productService.GetProductBySKUAsync(productDTO.ProductSKU);

            //     if (productWithSKUExists != null) return BadRequest("Product with this SKU has exists!");
            // }

            // var data = _mapper.Map<ProductDTOs, Product>(productDTO);

            // // Update product by productDTO
            // product.ProductBrandId = productDTO.ProductBrandId;
            // product.ProductSKUs = _mapper.Map<ProductDTOs, Product>(productDTO).ProductSKUs;
            // product.ProductOptions = _mapper.Map<ProductDTOs, Product>(productDTO).ProductOptions;
            // product.ProductTypeId = productDTO.ProductTypeId;
            // product.Description = productDTO.Description;
            // product.Name = productDTO.Name;
            // product.ProductSKU = productDTO.ProductSKU;
            // product.ImportPrice = productDTO.ImportPrice;

            // var productUpdatedResult = await _productService.UpdateProduct(product);

            // if (productUpdatedResult == null) return BadRequest("Error update product.");

            return Ok(product);
        }
    }
}