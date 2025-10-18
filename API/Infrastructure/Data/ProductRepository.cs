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

                // Generate slug with ProductSKU for better uniqueness
                if (string.IsNullOrWhiteSpace(product.Slug))
                {
                    product.Slug = await GenerateUniqueSlugAsync(product.Name, product.ProductSKU);
                }
                else
                {
                    // If slug is provided manually, still check for uniqueness
                    var slugExists = await _context.Products.AnyAsync(p => p.Slug == product.Slug);
                    if (slugExists)
                    {
                        _logger.LogWarning("Provided slug '{Slug}' already exists. Generating unique slug.", product.Slug);
                        product.Slug = await GenerateUniqueSlugAsync(product.Name, product.ProductSKU);
                    }
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

        public async Task<Product> UpdateProductAsync(int productId, Product updatedProduct)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // STEP 1: Load existing product with all relationships
                var existingProduct = await _context.Products
                    .Include(p => p.ProductOptions)
                        .ThenInclude(o => o.ProductOptionValues)
                    .Include(p => p.ProductSKUs)
                        .ThenInclude(s => s.ProductSKUValues)
                    .Include(p => p.ProductSKUs)
                        .ThenInclude(s => s.Photos)
                    .Include(p => p.Photos)
                    .FirstOrDefaultAsync(p => p.Id == productId);

                if (existingProduct == null)
                {
                    throw new ArgumentException($"Product with ID {productId} not found");
                }

                _logger.LogInformation("Updating product: ID={ProductId}, Name={ProductName}",
                    productId, existingProduct.Name);

                // STEP 2: Validate updated data
                ValidateProduct(updatedProduct);

                // STEP 3: Verify ProductType exists if changed
                if (updatedProduct.ProductTypeId != existingProduct.ProductTypeId)
                {
                    var productType = await _context.ProductTypes.FindAsync(updatedProduct.ProductTypeId);
                    if (productType == null)
                    {
                        throw new ArgumentException($"ProductType with ID {updatedProduct.ProductTypeId} not found");
                    }
                }

                // STEP 4: Update basic properties
                UpdateBasicProperties(existingProduct, updatedProduct);

                // STEP 5: Update ProductOptions (Delete old → Add new)
                UpdateProductOptions(existingProduct, updatedProduct);

                // STEP 6: Update ProductSKUs (Delete old → Add new)
                UpdateProductSKUs(existingProduct, updatedProduct);

                // STEP 7: Update Product Photos (Delete old → Add new)
                UpdateProductPhotos(existingProduct, updatedProduct);

                // STEP 8: Save changes
                await _context.SaveChangesAsync();

                // STEP 9: Commit transaction
                await transaction.CommitAsync();

                _logger.LogInformation(
                    "Product updated successfully: ID={ProductId}, Name={ProductName}",
                    existingProduct.Id, existingProduct.Name);

                // STEP 10: Reload with fresh data
                return await _context.Products
                    .Include(p => p.ProductType)
                    .Include(p => p.ProductOptions)
                        .ThenInclude(o => o.ProductOptionValues)
                    .Include(p => p.ProductSKUs)
                        .ThenInclude(s => s.ProductSKUValues)
                            .ThenInclude(sv => sv.ProductOptionValue)
                    .Include(p => p.ProductSKUs)
                        .ThenInclude(s => s.Photos)
                    .Include(p => p.Photos)
                    .FirstOrDefaultAsync(p => p.Id == productId);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error updating product: ID={ProductId}", productId);
                throw;
            }
        }

        private void UpdateBasicProperties(Product existing, Product updated)
        {
            existing.Name = updated.Name;
            existing.Description = updated.Description;
            existing.ProductSKU = updated.ProductSKU;
            existing.ImportPrice = updated.ImportPrice;
            existing.ProductTypeId = updated.ProductTypeId;
            existing.Style = updated.Style;
            existing.Season = updated.Season;
            existing.Material = updated.Material;
            existing.IsTrending = updated.IsTrending;

            // Update slug if provided
            if (!string.IsNullOrWhiteSpace(updated.Slug))
            {
                existing.Slug = updated.Slug;
            }
        }

        private void UpdateProductOptions(Product existing, Product updated)
        {
            // Remove all old options and values (cascade delete will handle ProductOptionValues)
            if (existing.ProductOptions != null && existing.ProductOptions.Any())
            {
                _context.ProductOptions.RemoveRange(existing.ProductOptions);
            }

            // Add new options
            if (updated.ProductOptions != null && updated.ProductOptions.Any())
            {
                existing.ProductOptions = new List<ProductOptions>();

                foreach (var option in updated.ProductOptions)
                {
                    if (option.ProductOptionValues == null || !option.ProductOptionValues.Any())
                    {
                        throw new ArgumentException($"Option '{option.OptionName}' must have at least one value");
                    }

                    var newOption = new ProductOptions
                    {
                        OptionName = option.OptionName,
                        ProductId = existing.Id,
                        Product = existing,
                        ProductOptionValues = option.ProductOptionValues.Select(v => new ProductOptionValues
                        {
                            ValueName = v.ValueName,
                            ValueTempId = v.ValueTempId
                        }).ToList()
                    };

                    existing.ProductOptions.Add(newOption);
                }
            }
        }

        private void UpdateProductSKUs(Product existing, Product updated)
        {
            // Remove all old SKUs (cascade delete will handle ProductSKUValues and Photos)
            if (existing.ProductSKUs != null && existing.ProductSKUs.Any())
            {
                _context.ProductSKUs.RemoveRange(existing.ProductSKUs);
            }

            // Add new SKUs
            if (updated.ProductSKUs != null && updated.ProductSKUs.Any())
            {
                existing.ProductSKUs = new List<ProductSKUs>();

                // Get main photo from Product for default SKU images
                var mainProductPhoto = updated.Photos?.FirstOrDefault(p => p.IsMain)
                    ?? existing.Photos?.FirstOrDefault(p => p.IsMain);

                // Build lookup for option values by ValueTempId
                // NOTE: Must use the NEW options we just added (existing.ProductOptions)
                var optionValuesLookup = existing.ProductOptions?
                    .SelectMany(opt => opt.ProductOptionValues)
                    .ToDictionary(v => v.ValueTempId, v => v);

                foreach (var sku in updated.ProductSKUs)
                {
                    var newSKU = new ProductSKUs
                    {
                        SKU = sku.SKU,
                        Quantity = sku.Quantity,
                        Price = sku.Price,
                        ImportPrice = sku.ImportPrice,
                        Barcode = sku.Barcode,
                        Weight = sku.Weight,
                        ImageUrl = string.IsNullOrEmpty(sku.ImageUrl) && mainProductPhoto != null
                            ? mainProductPhoto.Url
                            : sku.ImageUrl,
                        ProductId = existing.Id,
                        Product = existing
                    };

                    // Add SKU Photos
                    if (sku.Photos != null && sku.Photos.Any())
                    {
                        newSKU.Photos = sku.Photos.Select(photo => new Photo
                        {
                            Url = photo.Url,
                            IsMain = photo.IsMain
                        }).ToList();
                    }

                    // Link SKU values to option values
                    if (sku.ProductSKUValues != null && optionValuesLookup != null)
                    {
                        newSKU.ProductSKUValues = new List<ProductSKUValues>();

                        foreach (var skuValue in sku.ProductSKUValues)
                        {
                            if (optionValuesLookup.TryGetValue(skuValue.ValueTempId, out var optionValue))
                            {
                                newSKU.ProductSKUValues.Add(new ProductSKUValues
                                {
                                    ValueTempId = skuValue.ValueTempId,
                                    ProductOptionValue = optionValue
                                });
                            }
                            else
                            {
                                throw new ArgumentException(
                                    $"SKU value with ValueTempId {skuValue.ValueTempId} not found in product options");
                            }
                        }
                    }

                    existing.ProductSKUs.Add(newSKU);
                }
            }
        }

        private void UpdateProductPhotos(Product existing, Product updated)
        {
            // Remove old product-level photos (not SKU photos)
            if (existing.Photos != null && existing.Photos.Any())
            {
                _context.Photos.RemoveRange(existing.Photos);
            }

            // Add new product-level photos
            if (updated.Photos != null && updated.Photos.Any())
            {
                existing.Photos = new List<Photo>();

                // Ensure only one main photo
                var mainPhotos = updated.Photos.Where(p => p.IsMain).ToList();
                if (mainPhotos.Count > 1)
                {
                    mainPhotos.First().IsMain = true;
                    foreach (var photo in mainPhotos.Skip(1))
                    {
                        photo.IsMain = false;
                    }
                }
                else if (!mainPhotos.Any() && updated.Photos.Any())
                {
                    updated.Photos.First().IsMain = true;
                }

                foreach (var photo in updated.Photos)
                {
                    existing.Photos.Add(new Photo
                    {
                        Url = photo.Url,
                        IsMain = photo.IsMain
                    });
                }
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

        private async Task<string> GenerateUniqueSlugAsync(string name, string productSKU = null)
        {
            var baseSlug = GenerateSlug(name);
            var slug = baseSlug;

            // Check if slug already exists
            var exists = await _context.Products.AnyAsync(p => p.Slug == slug);

            if (!exists)
            {
                return slug;
            }

            if (!string.IsNullOrWhiteSpace(productSKU))
            {
                var skuSlug = GenerateSlug(productSKU);
                slug = $"{baseSlug}-{skuSlug}";

                exists = await _context.Products.AnyAsync(p => p.Slug == slug);
                if (!exists)
                {
                    _logger.LogInformation("Slug collision detected. Generated unique slug using SKU: {Slug}", slug);
                    return slug;
                }
            }

            var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            slug = $"{baseSlug}-{timestamp}";

            exists = await _context.Products.AnyAsync(p => p.Slug == slug);
            if (!exists)
            {
                _logger.LogInformation("Slug collision detected. Generated unique slug using timestamp: {Slug}", slug);
                return slug;
            }

            var counter = 1;
            slug = $"{baseSlug}-{counter}";

            while (await _context.Products.AnyAsync(p => p.Slug == slug))
            {
                counter++;
                slug = $"{baseSlug}-{counter}";
            }

            _logger.LogWarning("Slug collision detected. Generated unique slug using counter: {Slug}", slug);
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