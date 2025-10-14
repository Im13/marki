using System.ComponentModel.DataAnnotations;
using Core;
using Core.Common;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private StoreContext _context;
        private readonly IPhotoService _photoService;
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductService> _logger;

        public ProductService(IUnitOfWork unitOfWork, StoreContext context, IPhotoService photoService, IProductRepository productRepository, ILogger<ProductService> logger)
        {
            _context = context;
            _unitOfWork = unitOfWork;
            _photoService = photoService;
            _productRepository = productRepository;
            _logger = logger;
            _productRepository = productRepository;
        }

        public async Task<Product> GetProductAsync(int id)
        {
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(id);
            _unitOfWork.ClearTracker();

            return product;
        }

        //Using only for edit product because of ClearTracker
        public async Task<Product> GetProductBySKUAsync(string productSKU)
        {
            var products = await _unitOfWork.Repository<Product>().ListAllAsync();
            _unitOfWork.ClearTracker();

            return products.SingleOrDefault(p => p.ProductSKU == productSKU);
        }

        public async Task<Result<Product>> UpdateProductAsync(int productId, Product product)
        {
            try
            {
                var updatedProduct = await _productRepository.UpdateProductAsync(productId, product);
                return Result<Product>.Success(updatedProduct);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validation error updating product: ID={ProductId}", productId);
                return Result<Product>.Failure(ex.Message);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Product not found: ID={ProductId}", productId);
                return Result<Product>.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating product: ID={ProductId}", productId);
                return Result<Product>.Failure("An error occurred while updating the product");
            }
        }

        public async Task<Product> UpdateProduct(Product product)
        {
            // Clear existing tracking
            _unitOfWork.ClearTracker();

            // Load existing product with all related entities
            var existingProduct = await _context.Products
                .Include(p => p.Photos)
                .Include(p => p.ProductOptions)
                    .ThenInclude(po => po.ProductOptionValues)
                .Include(p => p.ProductSKUs)
                    .ThenInclude(sku => sku.ProductSKUValues)
                .FirstOrDefaultAsync(p => p.Id == product.Id);

            if (existingProduct == null) return null;

            // 1. Update basic properties
            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.ProductSKU = product.ProductSKU;
            existingProduct.ProductTypeId = product.ProductTypeId;
            existingProduct.Slug = product.Slug;
            existingProduct.ImportPrice = product.ImportPrice;

            // Photo photoToFind = new Photo();

            // //Get photos with productId
            // var spec = new PhotosWithProductFilterSpecification(product.Id);
            // var photos = await _unitOfWork.Repository<Photo>().ListAsync(spec);

            // var photosToDelete = photos.Except(product.Photos).ToList();

            // 1. Handle Photos
            var photosToDelete = existingProduct.Photos
                .Where(ep => !product.Photos.Any(p => p.Id == ep.Id))
                .ToList();

            foreach (var photo in photosToDelete)
            {
                _unitOfWork.Repository<Photo>().Delete(photo);

                //Need delete in Cloudinary
                var deleteResult = await _photoService.DeletePhotoAsync(photo.PublicId);

                if (deleteResult.Error != null)
                {
                    return null;
                }
            }

            // Add new photos
            foreach (var newPhoto in product.Photos.Where(p => p.Id == 0))
            {
                existingProduct.Photos.Add(newPhoto);
            }

            // 3. Handle ProductOptions
            // foreach (var existingOption in existingProduct.ProductOptions.ToList())
            // {
            //     var updatedOption = product.ProductOptions
            //         .FirstOrDefault(o => o.Id == existingOption.Id);

            //     if (updatedOption == null)
            //     {
            //         existingProduct.ProductOptions.Remove(existingOption);
            //     }
            //     else
            //     {
            //         existingOption.Name = updatedOption.Name;
            //         // Add other option properties

            //         // Handle option values
            //         foreach (var existingValue in existingOption.ProductOptionValues.ToList())
            //         {
            //             var updatedValue = updatedOption.ProductOptionValues
            //                 .FirstOrDefault(v => v.Id == existingValue.Id);

            //             if (updatedValue == null)
            //             {
            //                 existingOption.ProductOptionValues.Remove(existingValue);
            //             }
            //             else
            //             {
            //                 existingValue.Value = updatedValue.Value;
            //                 // Add other value properties
            //             }
            //         }

            //         // Add new values
            //         foreach (var newValue in updatedOption.ProductOptionValues
            //             .Where(v => !existingOption.ProductOptionValues
            //                 .Any(ev => ev.Id == v.Id)))
            //         {
            //             existingOption.ProductOptionValues.Add(newValue);
            //         }
            //     }
            // }

            // // Add new options
            // foreach (var newOption in product.ProductOptions
            //     .Where(o => !existingProduct.ProductOptions
            //         .Any(eo => eo.Id == o.Id)))
            // {
            //     existingProduct.ProductOptions.Add(newOption);
            // }

            _unitOfWork.Repository<Product>().Update(existingProduct);

            // UPDATE IMAGE FOR SKUS
            // if(product.ProductSKUs.Count > 0)
            // {
            //     photoToFind = await _unitOfWork.Repository<Photo>().GetByIdAsync(product.ProductSKUs.First().Photos.First().Id);

            //     if(photoToFind == null) {
            //         photoToFind = new Photo() 
            //         {
            //             IsMain = product.ProductSKUs.First().Photos.First().IsMain,
            //             PublicId = product.ProductSKUs.First().Photos.First().PublicId,
            //             Url = product.ProductSKUs.First().Photos.First().Url
            //         };

            //         _unitOfWork.Repository<Photo>().Add(photoToFind);
            //     }
            // }

            // foreach(var sku in product.ProductSKUs)
            // {
            //     foreach(var skuValue in sku.ProductSKUValues)
            //     {
            //         _unitOfWork.Repository<ProductSKUValues>().Update(skuValue);
            //     }

            //     sku.Photos.Add(photoToFind);

            //     _unitOfWork.Repository<ProductSKUs>().Update(sku);
            // }

            var result = await _unitOfWork.Complete();

            if (result <= 0) return null;

            return product;
        }

        public async Task<Product> CreateProduct(Product prod)
        {
            var product = await _productRepository.CreateProductAsync(prod);

            return prod;
        }

        public async Task<bool> DeleteProducts(List<Product> products)
        {
            if (products.Count == 0) return false;

            foreach (var product in products)
            {
                if (product.Id == 0) return false;

                product.IsDeleted = true;
                _unitOfWork.Repository<Product>().Update(product);
            }

            var result = await _unitOfWork.Complete();

            if (result <= 0) return false;

            return true;
        }

        public async Task<ProductSKUs> GetProductSKU(int skuId)
        {
            var productSku = await _context.ProductSKUs
                .Include(x => x.Product)
                .Include(x => x.ProductSKUValues)
                .ThenInclude(c => c.ProductOptionValue)
                .ThenInclude(pov => pov.ProductOption)
                .SingleOrDefaultAsync(p => p.Id == skuId);

            if (productSku == null || productSku.Product == null || productSku.Product.IsDeleted == true) return null;

            return productSku;
        }

        public async Task<Product> GetBySlug(string slug)
        {
            return await _context.Products
                .Include(p => p.Photos)
                .Include(p => p.ProductSKUs)
                .ThenInclude(p => p.ProductSKUValues)
                .ThenInclude(p => p.ProductOptionValue)
                .Include(p => p.ProductOptions)
                .ThenInclude(o => o.ProductOptionValues)
                .FirstOrDefaultAsync(p => p.Slug == slug);
        }
    }
}