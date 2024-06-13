using API.DTOs;
using API.DTOs.Product;
using API.Errors;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Admin
{
    public class AdminProductController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IProductService _productService;
        public AdminProductController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        // [HttpPost]
        // public async Task<ActionResult<ProductToReturnDTO>> CreateProduct(ProductDTO productDTO) 
        // {
        //     var product = _mapper.Map<ProductDTO,Product>(productDTO);

        //     var productWithSKUExists = await _productService.GetProductBySKUAsync(productDTO.ProductSKU);

        //     if (productWithSKUExists != null) return BadRequest("Product with this SKU has exists!");

        //     var resProduct = await _productService.CreateProduct(product);

        //     if(resProduct == null) return BadRequest(new ApiResponse(400, "Problem creating product")); 

        //     return Ok(resProduct);
        // }

        [HttpPost("create")]
        public async Task<ActionResult<ProductToReturnDTO>> CreateProducts(ProductDTOs productDTO) 
        {
            var options = _mapper.Map<List<ProductOptionDTO>,List<ProductOptions>>(productDTO.ProductOptions);
            var product = _mapper.Map<ProductDTOs,Product>(productDTO);

            // var productWithSKUExists = await _productService.GetProductBySKUAsync(productDTO.ProductSKU);

            // if (productWithSKUExists != null) return BadRequest("Product with this SKU has exists!");

            // var savedOptions = _productService.CreateProductOptions(options);
            var createdProduct = await _productService.CreateProduct(product, options);

            return Ok(product);
        }
    }
}