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

        public async Task<bool> SoftDeleteProductAsync(int productId)
        {
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(productId);
            if (product == null) return false;
            if (product.IsDeleted) return true;

            product.IsDeleted = true;
            _unitOfWork.Repository<Product>().Update(product);

            var result = await _unitOfWork.Complete();
            return result > 0;
        }

        public async Task<bool> SoftDeleteProductsAsync(IEnumerable<int> productIds)
        {
            var idList = productIds?.Distinct().ToList();
            if (idList == null || idList.Count == 0) return false;

            foreach (var id in idList)
            {
                var product = await _unitOfWork.Repository<Product>().GetByIdAsync(id);
                if (product == null) continue;
                if (product.IsDeleted) continue;
                product.IsDeleted = true;
                _unitOfWork.Repository<Product>().Update(product);
            }

            var result = await _unitOfWork.Complete();
            return result > 0;
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