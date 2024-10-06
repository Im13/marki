using API.DTOs;
using API.DTOs.Product;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specification;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<Product> _genericProductRepo;
        private readonly IGenericRepository<ProductType> _productTypeRepo;
        private readonly IMapper _mapper;
        private readonly IProductService _productService;
        private readonly IPhotoService _photoService;
        private readonly IProductRepository _productRepo;

        public ProductsController(IGenericRepository<Product> genericProductRepo,
        IGenericRepository<ProductType> productTypeRepo,
        IMapper mapper,
        IProductService productService,
        IProductRepository productRepo,
        IPhotoService photoService)
        {
            _productTypeRepo = productTypeRepo;
            _mapper = mapper;
            _genericProductRepo = genericProductRepo;
            _productService = productService;
            _photoService = photoService;
            _productRepo = productRepo;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDTO>> GetProduct(int id)
        {
            var spec = new ProductsWithTypesSpecification(id);

            var product = await _genericProductRepo.GetEntityWithSpec(spec);

            if(product == null) return NotFound(new ApiResponse(404));

            return _mapper.Map<Product,ProductToReturnDTO>(product);
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            var types = await _productTypeRepo.ListAllAsync();
            return Ok(types);
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDTO>> AddPhoto(IFormFile file)
        {
            if(file == null) return BadRequest("No file detected!");
            var result = await _photoService.AddPhotoAsync(file);

            if (result.Error != null)
            {
                return BadRequest(result.Error.Message);
            }

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            return _mapper.Map<Photo,PhotoDTO>(photo);
        }

        [HttpGet("new-arrivals")]
        public async Task<ActionResult<ProductDTOs>> GetNewArrivals([FromQuery]ProductSpecParams productParams)
        {
            var spec = new ProductsWithTypesSpecification(productParams);

            var countSpec = new ProductWithFiltersForCountSpecification(productParams);

            var totalItems = await _genericProductRepo.CountAsync(countSpec);

            var products = await _productRepo.GetProductForClientWithSpec(spec);

            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductForClientDTO>>(products);

            return Ok(new Pagination<ProductForClientDTO>(productParams.PageIndex, productParams.PageSize, totalItems, data));
        }
    }
}