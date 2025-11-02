using API.DTOs;
using API.DTOs.Product;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specification;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;

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
        private readonly StoreContext _context;

        public ProductsController(IGenericRepository<Product> genericProductRepo,
        IGenericRepository<ProductType> productTypeRepo,
        IMapper mapper,
        IProductService productService,
        IProductRepository productRepo,
        IPhotoService photoService,
        StoreContext context)
        {
            _productTypeRepo = productTypeRepo;
            _mapper = mapper;
            _genericProductRepo = genericProductRepo;
            _productService = productService;
            _photoService = photoService;
            _productRepo = productRepo;
            _context = context;
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
            var result = await _photoService.AddBannerPhotoWithResponsiveAsync(file);

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

        [HttpPost("add-original-photo")]
        public async Task<ActionResult<PhotoDTO>> AddOriginalPhoto(IFormFile file)
        {
            if(file == null) return BadRequest("No file detected!");
            var result = await _photoService.AddOriginalPhotoAsync(file);

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

        [HttpGet]
        public async Task<ActionResult<ProductDTOs>> GetProducts([FromQuery]ProductSpecParams productParams)
        {
            var spec = new ProductsWithTypesSpecification(productParams);

            var countSpec = new ProductWithFiltersForCountSpecification(productParams);

            var totalItems = await _genericProductRepo.CountAsync(countSpec);

            var products = await _productRepo.GetProductForClientWithSpec(spec);

            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductForClientDTO>>(products);

            return Ok(new Pagination<ProductForClientDTO>(productParams.PageIndex, productParams.PageSize, totalItems, data));
        }

        [HttpGet("slug/{slug}")]
        public async Task<ActionResult<ProductDTOs>> GetBySlug(string slug)
        {
            if(string.IsNullOrEmpty(slug)) return BadRequest();

            var product = await _productService.GetBySlug(slug);

            if(product == null) return NotFound("Cannot find product");

            var dto = _mapper.Map<Product,ProductForClientDTO>(product);

            return Ok(dto);
        }

        // ENDPOINT ĐỂ DEBUG RAW DATA
        [HttpGet("debug-slug/{slug}")]
        public async Task<ActionResult> DebugGetBySlug(string slug)
        {
            if(string.IsNullOrEmpty(slug)) return BadRequest();

            var product = await _productService.GetBySlug(slug);

            if(product == null) return NotFound("Cannot find product");

            // Trả về raw entity (không qua AutoMapper) để debug
            return Ok(new
            {
                product.Id,
                product.Name,
                ProductSKUsCount = product.ProductSKUs?.Count ?? 0,
                ProductSKUs = product.ProductSKUs?.Select(sku => new
                {
                    sku.Id,
                    sku.SKU,
                    ProductSKUValuesCount = sku.ProductSKUValues?.Count ?? 0,
                    ProductSKUValues = sku.ProductSKUValues?.Select(v => new
                    {
                        v.Id,
                        ProductOptionValue = v.ProductOptionValue == null ? null : new
                        {
                            v.ProductOptionValue.Id,
                            v.ProductOptionValue.ValueName,
                            ProductOption = v.ProductOptionValue.ProductOption == null ? null : new
                            {
                                v.ProductOptionValue.ProductOption.Id,
                                v.ProductOptionValue.ProductOption.OptionName
                            }
                        }
                    }).ToList()
                }).ToList(),
                ProductOptionsCount = product.ProductOptions?.Count ?? 0,
                ProductOptions = product.ProductOptions?.Select(opt => new
                {
                    opt.Id,
                    opt.OptionName,
                    ValuesCount = opt.ProductOptionValues?.Count ?? 0,
                    Values = opt.ProductOptionValues?.Select(v => new
                    {
                        v.Id,
                        v.ValueName
                    }).ToList()
                }).ToList()
            });
        }

        // FIX DATA ENDPOINT - Tạo ProductSKUValues cho product dựa trên SKU code
        [HttpPost("fix-sku-values/{productId}")]
        public async Task<ActionResult> FixProductSKUValues(int productId)
        {
            try
            {
                var product = await _context.Products
                    .Include(p => p.ProductSKUs)
                        .ThenInclude(s => s.ProductSKUValues)
                    .Include(p => p.ProductOptions)
                        .ThenInclude(o => o.ProductOptionValues)
                    .FirstOrDefaultAsync(p => p.Id == productId);

                if (product == null)
                    return NotFound("Product not found");

                if (product.ProductOptions == null || !product.ProductOptions.Any())
                    return BadRequest("Product has no options");

                // Tạo color mapping để normalize tên màu
                var colorMapping = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    { "Den", "Đen" },
                    { "Xanhla", "Xanh lá" },
                    { "Xanhduong", "Xanh dương" },
                    { "Xanhnhat", "Xanh nhạt" },
                    { "Trang", "Trắng" },
                    { "Do", "Đỏ" },
                    { "Vang", "Vàng" },
                    { "Hong", "Hồng" },
                    { "Xam", "Xám" },
                    { "Nau", "Nâu" },
                    { "Cam", "Cam" },
                    { "Tim", "Tím" }
                };

                // Parse SKU codes và tạo ProductSKUValues
                var fixedCount = 0;
                var errors = new List<string>();
                var details = new List<object>();

                foreach (var sku in product.ProductSKUs)
                {
                    try
                    {
                        // Xóa ProductSKUValues cũ nếu có
                        if (sku.ProductSKUValues != null && sku.ProductSKUValues.Any())
                        {
                            _context.ProductSKUValues.RemoveRange(sku.ProductSKUValues);
                        }

                        // Parse SKU code: "Halter-neck knitted topMXanhla" 
                        // Format: {ProductName}{Size}{Color}
                        var skuCode = sku.SKU;
                        var productName = product.Name; // "Halter-neck knitted top"
                        
                        // Lấy phần sau product name
                        var suffix = skuCode.Replace(productName, "").Replace(" ", "");
                        
                        // Tìm Size và Color options
                        var sizeOption = product.ProductOptions.FirstOrDefault(o => 
                            o.OptionName.Equals("Size", StringComparison.OrdinalIgnoreCase));
                        var colorOption = product.ProductOptions.FirstOrDefault(o => 
                            o.OptionName.Equals("Màu sắc", StringComparison.OrdinalIgnoreCase) ||
                            o.OptionName.Equals("Color", StringComparison.OrdinalIgnoreCase));

                        if (sizeOption == null || colorOption == null)
                        {
                            errors.Add($"SKU {sku.Id}: Cannot find Size or Color option");
                            details.Add(new
                            {
                                skuId = sku.Id,
                                skuCode = sku.SKU,
                                error = "Missing Size or Color option",
                                success = false
                            });
                            continue;
                        }

                        // Parse size (S, M, L, XL, XXL)
                        ProductOptionValues sizeValue = null;
                        var remainingAfterSize = suffix;
                        
                        foreach (var size in sizeOption.ProductOptionValues.OrderByDescending(v => v.ValueName.Length))
                        {
                            if (suffix.StartsWith(size.ValueName, StringComparison.OrdinalIgnoreCase))
                            {
                                sizeValue = size;
                                remainingAfterSize = suffix.Substring(size.ValueName.Length);
                                break;
                            }
                        }

                        if (sizeValue == null)
                        {
                            errors.Add($"SKU {sku.Id} ({skuCode}): Cannot parse size from '{suffix}'");
                            details.Add(new
                            {
                                skuId = sku.Id,
                                skuCode = sku.SKU,
                                error = $"Cannot parse size from '{suffix}'",
                                availableSizes = sizeOption.ProductOptionValues.Select(v => v.ValueName).ToList(),
                                success = false
                            });
                            continue;
                        }

                        // Parse color - thử normalize trước
                        var colorCode = remainingAfterSize;
                        var normalizedColorName = colorMapping.ContainsKey(colorCode) 
                            ? colorMapping[colorCode] 
                            : colorCode;

                        var colorValue = colorOption.ProductOptionValues.FirstOrDefault(v =>
                            v.ValueName.Equals(normalizedColorName, StringComparison.OrdinalIgnoreCase) ||
                            v.ValueName.Replace(" ", "").Equals(colorCode, StringComparison.OrdinalIgnoreCase));

                        if (colorValue == null)
                        {
                            errors.Add($"SKU {sku.Id} ({skuCode}): Cannot parse color from '{colorCode}' (normalized: '{normalizedColorName}')");
                            details.Add(new
                            {
                                skuId = sku.Id,
                                skuCode = sku.SKU,
                                error = $"Cannot parse color from '{colorCode}'",
                                normalizedColorName,
                                availableColors = colorOption.ProductOptionValues.Select(v => v.ValueName).ToList(),
                                success = false
                            });
                            continue;
                        }

                        // Tạo ProductSKUValues mới
                        var skuValue1 = new ProductSKUValues
                        {
                            ProductOptionValue = sizeValue
                        };
                        
                        var skuValue2 = new ProductSKUValues
                        {
                            ProductOptionValue = colorValue
                        };

                        // Add to context
                        await _context.ProductSKUValues.AddAsync(skuValue1);
                        await _context.ProductSKUValues.AddAsync(skuValue2);

                        // Add to SKU's collection
                        if (sku.ProductSKUValues == null)
                            sku.ProductSKUValues = new List<ProductSKUValues>();
                        
                        sku.ProductSKUValues.Add(skuValue1);
                        sku.ProductSKUValues.Add(skuValue2);

                        details.Add(new
                        {
                            skuId = sku.Id,
                            skuCode = sku.SKU,
                            size = sizeValue.ValueName,
                            color = colorValue.ValueName,
                            colorCode,
                            success = true
                        });

                        fixedCount++;
                    }
                    catch (Exception ex)
                    {
                        errors.Add($"SKU {sku.Id}: {ex.Message}");
                        details.Add(new
                        {
                            skuId = sku.Id,
                            skuCode = sku.SKU,
                            error = ex.Message,
                            success = false
                        });
                    }
                }

                if (fixedCount > 0)
                {
                    await _context.SaveChangesAsync();
                }

                return Ok(new
                {
                    success = fixedCount > 0,
                    fixedCount,
                    totalSKUs = product.ProductSKUs.Count,
                    errors,
                    details
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message, stackTrace = ex.StackTrace });
            }
        }
    }
}
