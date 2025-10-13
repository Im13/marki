using System.ComponentModel.DataAnnotations;
using Core;
using Core.Entities;
using Core.Interfaces;
using Core.Specification;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly ILogger<ProductRepository> _logger;

        public ProductRepository(StoreContext context, ILogger<ProductRepository> logger) : base(context)
        {
            _logger = logger;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products.Include(p => p.ProductSKUs).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IReadOnlyList<Product>> GetProductsAsync()
        {
            return await _context.Products.Include(p => p.ProductType).ToListAsync();
        }

        public async Task<IReadOnlyList<ProductType>> GetProductTypesAsync()
        {
            return await _context.ProductTypes.ToListAsync();
        }

        public async Task<List<Product>> GetProductsWithSpec(ISpecification<Product> spec)
        {
            var products = await SpecificationEvaluator<Product>.GetQuery(_context.Set<Product>().AsQueryable(), spec)
                .Include(p => p.ProductSKUs)
                .ThenInclude(ps => ps.ProductSKUValues)
                .ThenInclude(psv => psv.ProductOptionValue)
                .ThenInclude(pov => pov.ProductOption)
                .Include(p => p.ProductSKUs).ThenInclude(ps => ps.Photos)
                .Include(p => p.ProductOptions).ThenInclude(po => po.ProductOptionValues)
                .Include(p => p.Photos)
                .ToListAsync();

            return products;
        }

        public async Task<List<Product>> GetProductForClientWithSpec(ISpecification<Product> spec)
        {
            var products = await SpecificationEvaluator<Product>.GetQuery(_context.Set<Product>().AsQueryable(), spec)
                .Include(p => p.Photos)
                .ToListAsync();

            return products;
        }

        public async Task<List<ProductSKUs>> GetProductSKUsWithSpec(ISpecification<ProductSKUs> spec)
        {
            var productSkus = await SpecificationEvaluator<ProductSKUs>.GetQuery(_context.Set<ProductSKUs>().AsQueryable(), spec)
                .Include(p => p.Product)
                .Include(p => p.ProductSKUValues).ThenInclude(ps => ps.ProductOptionValue).ThenInclude(pov => pov.ProductOption)
                .ToListAsync();

            return productSkus;
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            // Transaction
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Validate 
                ValidateProduct(product);
                _logger.LogInformation("Creating product: {ProductName}", product.Name);

                //Generate slug
                if (string.IsNullOrWhiteSpace(product.Slug))
                {
                    product.Slug = await GenerateUniqueSlugAsync(product.Name);
                }

                // Load product type
                var productType = await _context.ProductTypes.FindAsync(product.ProductTypeId);
                if (productType == null)
                {
                    throw new ArgumentException($"ProductType with ID {product.ProductTypeId} not found");
                }

                // Set default values
                product.IsDeleted = false;
                product.IsTrending = false;

                // Process ProductOptions
                ProcessProductOptions(product);

                // STEP 6: Process SKUs
                ProcessProductSKUs(product);

                // STEP 7: Process Photos
                ProcessPhotos(product);

                // STEP 8: Add to context
                await _context.Products.AddAsync(product);

                // STEP 9: Save changes
                await _context.SaveChangesAsync();

                // STEP 10: Commit transaction
                await transaction.CommitAsync();

                _logger.LogInformation(
                    "Product created successfully: ID={ProductId}, Name={ProductName}",
                    product.Id, product.Name);

                return product;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a product.");
                await transaction.RollbackAsync();
                throw; // Re-throw the exception after logging it
            }
        }

        private void ValidateProduct(Product product)
        {
            var errors = new List<string>();

            if (product == null)
                throw new ArgumentNullException(nameof(product));

            if (string.IsNullOrWhiteSpace(product.Name))
                errors.Add("Product name is required");

            if (product.Name?.Length > 200)
                errors.Add("Product name must be less than 200 characters");

            if (product.ProductTypeId <= 0)
                errors.Add("ProductType is required");

            if (product.ImportPrice <= 0)
                errors.Add("Import price must be greater than 0");

            if (product.ProductSKUs == null || !product.ProductSKUs.Any())
                errors.Add("At least one SKU is required");

            if (product.ProductSKUs != null)
            {
                // Validate SKU codes are unique
                var duplicateSKUs = product.ProductSKUs
                    .GroupBy(s => s.SKU)
                    .Where(g => g.Count() > 1)
                    .Select(g => g.Key)
                    .ToList();

                if (duplicateSKUs.Any())
                    errors.Add($"Duplicate SKU codes: {string.Join(", ", duplicateSKUs)}");

                // Validate SKU prices
                if (product.ProductSKUs.Any(s => s.Price <= 0))
                    errors.Add("All SKU prices must be greater than 0");
            }

            if (errors.Any())
            {
                throw new ValidationException(string.Join("; ", errors));
            }
        }

        private async Task<string> GenerateUniqueSlugAsync(string name)
        {
            var baseSlug = GenerateSlug(name);
            var slug = baseSlug;
            var counter = 1;

            // Ensure slug is unique
            while (await _context.Products.AnyAsync(p => p.Slug == slug))
            {
                slug = $"{baseSlug}-{counter}";
                counter++;
            }

            return slug;
        }

        private string GenerateSlug(string name)
        {
            // Convert to lowercase and replace spaces with hyphens
            var slug = name.ToLower()
                .Replace(" ", "-")
                .Replace("á", "a").Replace("à", "a").Replace("ả", "a").Replace("ã", "a").Replace("ạ", "a")
                .Replace("ă", "a").Replace("ắ", "a").Replace("ằ", "a").Replace("ẳ", "a").Replace("ẵ", "a").Replace("ặ", "a")
                .Replace("â", "a").Replace("ấ", "a").Replace("ầ", "a").Replace("ẩ", "a").Replace("ẫ", "a").Replace("ậ", "a")
                .Replace("đ", "d")
                .Replace("é", "e").Replace("è", "e").Replace("ẻ", "e").Replace("ẽ", "e").Replace("ẹ", "e")
                .Replace("ê", "e").Replace("ế", "e").Replace("ề", "e").Replace("ể", "e").Replace("ễ", "e").Replace("ệ", "e")
                .Replace("í", "i").Replace("ì", "i").Replace("ỉ", "i").Replace("ĩ", "i").Replace("ị", "i")
                .Replace("ó", "o").Replace("ò", "o").Replace("ỏ", "o").Replace("õ", "o").Replace("ọ", "o")
                .Replace("ô", "o").Replace("ố", "o").Replace("ồ", "o").Replace("ổ", "o").Replace("ỗ", "o").Replace("ộ", "o")
                .Replace("ơ", "o").Replace("ớ", "o").Replace("ờ", "o").Replace("ở", "o").Replace("ỡ", "o").Replace("ợ", "o")
                .Replace("ú", "u").Replace("ù", "u").Replace("ủ", "u").Replace("ũ", "u").Replace("ụ", "u")
                .Replace("ư", "u").Replace("ứ", "u").Replace("ừ", "u").Replace("ử", "u").Replace("ữ", "u").Replace("ự", "u")
                .Replace("ý", "y").Replace("ỳ", "y").Replace("ỷ", "y").Replace("ỹ", "y").Replace("ỵ", "y");

            // Remove special characters
            slug = System.Text.RegularExpressions.Regex.Replace(slug, @"[^a-z0-9\-]", "");

            // Remove consecutive hyphens
            slug = System.Text.RegularExpressions.Regex.Replace(slug, @"-+", "-");

            // Trim hyphens from ends
            slug = slug.Trim('-');

            return slug;
        }

        private void ProcessProductOptions(Product product)
        {
            if (product.ProductOptions == null || !product.ProductOptions.Any())
                return;

            foreach (var option in product.ProductOptions)
            {
                // Ensure option has values
                if (option.ProductOptionValues == null || !option.ProductOptionValues.Any())
                {
                    throw new ArgumentException($"Option '{option.OptionName}' must have at least one value");
                }

                // Set ProductId (will be set by EF, but explicit is clearer)
                option.Product = product;
            }
        }
        private void ProcessProductSKUs(Product product)
        {
            if (product.ProductSKUs == null || !product.ProductSKUs.Any())
                return;

            // Get main photo for default SKU images
            var mainPhoto = product.Photos?.FirstOrDefault(p => p.IsMain);

            // Build lookup dictionary for ProductOptionValues by ValueTempId
            var optionValuesLookup = product.ProductOptions?
                .SelectMany(opt => opt.ProductOptionValues)
                .ToDictionary(v => v.ValueTempId, v => v);

            foreach (var sku in product.ProductSKUs)
            {
                // Set product reference
                sku.Product = product;

                // Set default image if not provided
                if (string.IsNullOrEmpty(sku.ImageUrl) && mainPhoto != null)
                {
                    sku.ImageUrl = mainPhoto.Url;
                }

                // Link SKU values to option values
                if (sku.ProductSKUValues != null && optionValuesLookup != null)
                {
                    foreach (var skuValue in sku.ProductSKUValues)
                    {
                        if (optionValuesLookup.TryGetValue(skuValue.ValueTempId, out var optionValue))
                        {
                            skuValue.ProductOptionValue = optionValue;
                        }
                        else
                        {
                            throw new ArgumentException(
                                $"SKU value with ValueTempId {skuValue.ValueTempId} not found in product options");
                        }
                    }
                }
            }
        }

        private void ProcessPhotos(Product product)
        {
            if (product.Photos == null || !product.Photos.Any())
                return;

            // Ensure only one main photo
            var mainPhotos = product.Photos.Where(p => p.IsMain).ToList();
            if (mainPhotos.Count > 1)
            {
                // Set first as main, rest as non-main
                mainPhotos.First().IsMain = true;
                foreach (var photo in mainPhotos.Skip(1))
                {
                    photo.IsMain = false;
                }
            }
            else if (!mainPhotos.Any())
            {
                // Set first photo as main
                product.Photos.First().IsMain = true;
            }
        }
    }
}