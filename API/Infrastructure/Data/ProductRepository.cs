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
                // STEP 1: Validate 
                ValidateProduct(product);
                _logger.LogInformation("Creating product: {ProductName}", product.Name);

                // STEP 2: Generate slug with ProductSKU for better uniqueness
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

                // STEP 3: Set default values
                product.IsDeleted = false;
                product.IsTrending = false;

                // STEP 4: Process ProductOptions
                ProcessProductOptions(product);

                // STEP 5: Process SKUs (với validation ProductSKUValues)
                ProcessProductSKUs(product);

                // STEP 6: Process Photos
                ProcessPhotos(product);

                // STEP 7: Add to context
                await _context.Products.AddAsync(product);

                // STEP 8: Save changes
                await _context.SaveChangesAsync();

                // STEP 9: Validate ProductSKUValues được tạo đúng
                await ValidateProductSKUValuesAfterSave(product);

                // STEP 10: Commit transaction
                await transaction.CommitAsync();

                _logger.LogInformation(
                    "Product created successfully: ID={ProductId}, Name={ProductName}, TotalSKUs={SkuCount}",
                    product.Id, product.Name, product.ProductSKUs?.Count ?? 0);

                return product;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating product: {ProductName}", product.Name);
                await transaction.RollbackAsync();
                throw;
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

                // STEP 2: Validate updated product
                ValidateProduct(updatedProduct);

                // STEP 3: Update basic properties
                existingProduct.Name = updatedProduct.Name;
                existingProduct.Description = updatedProduct.Description;
                existingProduct.ProductTypeId = updatedProduct.ProductTypeId;
                existingProduct.ProductSKU = updatedProduct.ProductSKU;
                existingProduct.ImportPrice = updatedProduct.ImportPrice;
                existingProduct.Style = updatedProduct.Style;
                existingProduct.Season = updatedProduct.Season;
                existingProduct.Material = updatedProduct.Material;
                existingProduct.IsTrending = updatedProduct.IsTrending;

                // STEP 4: Update slug if name changed
                if (updatedProduct.Name != existingProduct.Name)
                {
                    existingProduct.Slug = await GenerateUniqueSlugAsync(
                        updatedProduct.Name, 
                        updatedProduct.ProductSKU);
                }

                // STEP 5: Update ProductOptions
                UpdateProductOptions(existingProduct, updatedProduct);

                // STEP 6: Update ProductSKUs
                UpdateProductSKUs(existingProduct, updatedProduct);

                // STEP 7: Update Photos
                UpdateProductPhotos(existingProduct, updatedProduct);

                // STEP 8: Save changes
                await _context.SaveChangesAsync();

                // STEP 9: Validate ProductSKUValues
                await ValidateProductSKUValuesAfterSave(existingProduct);

                // STEP 10: Commit transaction
                await transaction.CommitAsync();

                _logger.LogInformation("Product updated successfully: ID={ProductId}", productId);

                return existingProduct;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating product: ID={ProductId}", productId);
                await transaction.RollbackAsync();
                throw;
            }
        }

        private void UpdateProductOptions(Product existing, Product updated)
        {
            // Remove old options and their values
            if (existing.ProductOptions != null && existing.ProductOptions.Any())
            {
                foreach (var option in existing.ProductOptions.ToList())
                {
                    if (option.ProductOptionValues != null)
                    {
                        _context.ProductOptionValues.RemoveRange(option.ProductOptionValues);
                    }
                }
                _context.ProductOptions.RemoveRange(existing.ProductOptions);
            }

            // Add new options
            if (updated.ProductOptions != null && updated.ProductOptions.Any())
            {
                existing.ProductOptions = new List<ProductOptions>();

                foreach (var option in updated.ProductOptions)
                {
                    var newOption = new ProductOptions
                    {
                        OptionName = option.OptionName,
                        Product = existing
                    };

                    if (option.ProductOptionValues != null && option.ProductOptionValues.Any())
                    {
                        newOption.ProductOptionValues = new List<ProductOptionValues>();

                        foreach (var value in option.ProductOptionValues)
                        {
                            newOption.ProductOptionValues.Add(new ProductOptionValues
                            {
                                ValueName = value.ValueName,
                                ValueTempId = value.ValueTempId,
                                ProductOption = newOption
                            });
                        }
                    }

                    existing.ProductOptions.Add(newOption);
                }
            }
        }

        private void UpdateProductSKUs(Product existing, Product updated)
        {
            // Remove old SKUs and their values/photos
            if (existing.ProductSKUs != null && existing.ProductSKUs.Any())
            {
                foreach (var sku in existing.ProductSKUs.ToList())
                {
                    if (sku.ProductSKUValues != null)
                    {
                        _context.ProductSKUValues.RemoveRange(sku.ProductSKUValues);
                    }
                    if (sku.Photos != null)
                    {
                        _context.Photos.RemoveRange(sku.Photos);
                    }
                }
                _context.ProductSKUs.RemoveRange(existing.ProductSKUs);
            }

            // Add new SKUs
            if (updated.ProductSKUs != null && updated.ProductSKUs.Any())
            {
                existing.ProductSKUs = new List<ProductSKUs>();

                var mainProductPhoto = existing.Photos?.FirstOrDefault(p => p.IsMain);

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

            if (product.ProductOptions != null && product.ProductOptions.Any())
            {
                var totalOptionValues = product.ProductOptions.Sum(o => o.ProductOptionValues?.Count ?? 0);
                
                if (totalOptionValues > 0)
                {
                    var allValidValueTempIds = product.ProductOptions
                        .Where(o => o.ProductOptionValues != null && o.ProductOptionValues.Any())
                        .SelectMany(o => o.ProductOptionValues)
                        .Where(v => v != null && v.ValueTempId > 0)
                        .Select(v => new { OptionName = v.ProductOption?.OptionName ?? "Unknown", ValueName = v.ValueName, ValueTempId = v.ValueTempId })
                        .ToList();
                        
                    foreach (var sku in product.ProductSKUs ?? Enumerable.Empty<ProductSKUs>())
                    {
                        if (sku.ProductSKUValues == null || !sku.ProductSKUValues.Any())
                        {
                            errors.Add($"SKU '{sku.SKU}' must have ProductSKUValues linking to ProductOptions");
                        }
                        else
                        {
                            var expectedCount = product.ProductOptions.Count;
                            var actualCount = sku.ProductSKUValues.Count;

                            if (actualCount != expectedCount)
                            {
                                errors.Add(
                                    $"SKU '{sku.SKU}' has {actualCount} ProductSKUValues but product has {expectedCount} ProductOptions. " +
                                    $"Each SKU must have exactly one value for each option.");
                            }

                            var validValueTempIds = product.ProductOptions
                                .Where(o => o.ProductOptionValues != null && o.ProductOptionValues.Any())
                                .SelectMany(o => o.ProductOptionValues)
                                .Where(v => v != null && v.ValueTempId > 0)
                                .Select(v => v.ValueTempId)
                                .ToHashSet();

                            foreach (var skuValue in sku.ProductSKUValues)
                            {
                                if (skuValue == null)
                                {
                                    errors.Add($"SKU '{sku.SKU}' has null ProductSKUValue.");
                                    continue;
                                }

                                if (skuValue.ValueTempId <= 0)
                                {
                                    errors.Add(
                                        $"SKU '{sku.SKU}' has invalid or missing ValueTempId in ProductSKUValues.");
                                }
                            }
                        }
                    }
                }
            }

            if (product.ProductSKUs != null)
            {
                var duplicateSKUs = product.ProductSKUs
                    .GroupBy(s => s.SKU)
                    .Where(g => g.Count() > 1)
                    .Select(g => g.Key)
                    .ToList();

                if (duplicateSKUs.Any())
                    errors.Add($"Duplicate SKU codes: {string.Join(", ", duplicateSKUs)}");

                if (product.ProductSKUs.Any(s => s.Price <= 0))
                    errors.Add("All SKU prices must be greater than 0");
            }

            if (errors.Any())
            {
                var errorMessage = string.Join("; ", errors);
                _logger.LogWarning("Product validation failed: {Errors}", errorMessage);
                throw new ValidationException(errorMessage);
            }
        }

        private async Task ValidateProductSKUValuesAfterSave(Product product)
        {
            var verifyProduct = await _context.Products
                .Include(p => p.ProductSKUs)
                    .ThenInclude(s => s.ProductSKUValues)
                .Include(p => p.ProductOptions)
                .FirstOrDefaultAsync(p => p.Id == product.Id);

            if (verifyProduct == null) return;

            var hasOptions = verifyProduct.ProductOptions != null && verifyProduct.ProductOptions.Any();
            
            if (!hasOptions) return;

            var errors = new List<string>();

            foreach (var sku in verifyProduct.ProductSKUs ?? Enumerable.Empty<ProductSKUs>())
            {
                if (sku.ProductSKUValues == null || !sku.ProductSKUValues.Any())
                {
                    errors.Add($"SKU ID {sku.Id} ('{sku.SKU}') has NO ProductSKUValues after save");
                }
                else
                {
                    _logger.LogInformation(
                        "SKU ID {SkuId} has {Count} ProductSKUValues", 
                        sku.Id, 
                        sku.ProductSKUValues.Count);
                }
            }

            if (errors.Any())
            {
                var errorMessage = string.Join("; ", errors);
                _logger.LogError("ProductSKUValues validation failed after save: {Errors}", errorMessage);
                throw new InvalidOperationException(
                    $"Product was saved but ProductSKUValues are missing. This indicates a data integrity issue. {errorMessage}");
            }
        }

        private async Task<string> GenerateUniqueSlugAsync(string name, string productSKU = null)
        {
            var baseSlug = GenerateSlug(name);
            var slug = baseSlug;

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

            slug = System.Text.RegularExpressions.Regex.Replace(slug, @"[^a-z0-9\-]", "");

            slug = System.Text.RegularExpressions.Regex.Replace(slug, @"-+", "-");

            slug = slug.Trim('-');

            return slug;
        }

        private void ProcessProductOptions(Product product)
        {
            if (product.ProductOptions == null || !product.ProductOptions.Any())
                return;

            foreach (var option in product.ProductOptions)
            {
                if (option.ProductOptionValues == null || !option.ProductOptionValues.Any())
                {
                    throw new ArgumentException($"Option '{option.OptionName}' must have at least one value");
                }

                option.Product = product;
            }
        }

        private void ProcessProductSKUs(Product product)
        {
            if (product.ProductSKUs == null || !product.ProductSKUs.Any())
                return;

            var mainPhoto = product.Photos?.FirstOrDefault(p => p.IsMain);

            var optionValuesLookupByTempId = product.ProductOptions?
                .SelectMany(opt => opt.ProductOptionValues)
                .ToDictionary(v => v.ValueTempId, v => v);

            var optionValuesLookupByName = product.ProductOptions?
                .SelectMany(opt => opt.ProductOptionValues
                    .Select(v => new { OptionName = opt.OptionName, ValueName = v.ValueName, Value = v }))
                .GroupBy(x => new { x.OptionName, x.ValueName })
                .ToDictionary(g => g.Key, g => g.First().Value);

            foreach (var sku in product.ProductSKUs)
            {
                sku.Product = product;

                if (string.IsNullOrEmpty(sku.ImageUrl) && mainPhoto != null)
                {
                    sku.ImageUrl = mainPhoto.Url;
                }

                if (sku.ProductSKUValues != null && (optionValuesLookupByTempId != null || optionValuesLookupByName != null))
                {
                    foreach (var skuValue in sku.ProductSKUValues)
                    {
                        ProductOptionValues optionValue = null;
                        bool found = false;

                        if (optionValuesLookupByTempId != null && 
                            skuValue.ValueTempId > 0 &&
                            optionValuesLookupByTempId.TryGetValue(skuValue.ValueTempId, out optionValue))
                        {
                            found = true;
                            _logger.LogDebug(
                                "Linked SKU value by ValueTempId: SKU={Sku}, ValueTempId={TempId}, OptionValue={OptionValue}",
                                sku.SKU, skuValue.ValueTempId, optionValue.ValueName);
                        }
                        if (!found && optionValuesLookupByName != null && 
                                 skuValue.ProductOptionValue != null &&
                                 skuValue.ProductOptionValue.ProductOption != null)
                        {
                            var key = new { 
                                OptionName = skuValue.ProductOptionValue.ProductOption.OptionName, 
                                ValueName = skuValue.ProductOptionValue.ValueName 
                            };
                            
                            if (optionValuesLookupByName.TryGetValue(key, out optionValue))
                            {
                                found = true;
                                skuValue.ValueTempId = optionValue.ValueTempId;
                                _logger.LogInformation(
                                    "Linked SKU value by OptionName+ValueName: SKU={Sku}, Option={Option}, Value={Value}, NewValueTempId={TempId}",
                                    sku.SKU, key.OptionName, key.ValueName, optionValue.ValueTempId);
                            }
                        }

                        if (!found && product.ProductOptions != null && 
                            sku.ProductSKUValues != null &&
                            sku.ProductSKUValues.Count == product.ProductOptions.Count)
                        {
                            var skuValueIndex = sku.ProductSKUValues.ToList().IndexOf(skuValue);
                            
                            if (skuValueIndex >= 0 && skuValueIndex < product.ProductOptions.Count)
                            {
                                var optionsList = product.ProductOptions.ToList();
                                var optionAtIndex = optionsList[skuValueIndex];
                                if (optionAtIndex.ProductOptionValues != null && optionAtIndex.ProductOptionValues.Any())
                                {
                                    optionValue = optionAtIndex.ProductOptionValues
                                        .FirstOrDefault(v => v.ValueTempId == skuValue.ValueTempId);
                                    
                                    if (optionValue == null && optionAtIndex.ProductOptionValues.Count > 0)
                                    {
                                        optionValue = optionAtIndex.ProductOptionValues.First();
                                        skuValue.ValueTempId = optionValue.ValueTempId;
                                        found = true;
                                        _logger.LogWarning(
                                            "Linked SKU value by position/index: SKU={Sku}, Index={Index}, Option={Option}, Value={Value}, NewValueTempId={TempId}",
                                            sku.SKU, skuValueIndex, optionAtIndex.OptionName, optionValue.ValueName, optionValue.ValueTempId);
                                    }
                                    else if (optionValue != null)
                                    {
                                        found = true;
                                        _logger.LogWarning(
                                            "Found matching value by position search: SKU={Sku}, Index={Index}, ValueTempId={TempId}, OptionValue={OptionValue}",
                                            sku.SKU, skuValueIndex, skuValue.ValueTempId, optionValue.ValueName);
                                    }
                                }
                            }
                        }

                        if (found && optionValue != null)
                        {
                            skuValue.ProductOptionValue = optionValue;
                        }
                        else
                        {
                            throw new ArgumentException(
                                $"Cannot link SKU value for SKU '{sku.SKU}' with ValueTempId {skuValue.ValueTempId}. " +
                                $"Could not find matching ProductOptionValue. " +
                                $"Available ValueTempIds: {string.Join(", ", optionValuesLookupByTempId?.Keys.OrderBy(x => x) ?? Enumerable.Empty<int>())}. " +
                                $"SKU has {sku.ProductSKUValues?.Count ?? 0} values, Product has {product.ProductOptions?.Count ?? 0} options.");
                        }
                    }
                }
            }
        }

        private void ProcessPhotos(Product product)
        {
            if (product.Photos == null || !product.Photos.Any())
                return;

            var mainPhotos = product.Photos.Where(p => p.IsMain).ToList();
            if (mainPhotos.Count > 1)
            {
                mainPhotos.First().IsMain = true;
                foreach (var photo in mainPhotos.Skip(1))
                {
                    photo.IsMain = false;
                }
            }
            else if (!mainPhotos.Any())
            {
                product.Photos.First().IsMain = true;
            }
        }
    }
}
