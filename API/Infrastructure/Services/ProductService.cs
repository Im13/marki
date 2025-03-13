using Core;
using Core.Entities;
using Core.Interfaces;
using Core.Specification;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private StoreContext _context;
        private readonly IPhotoService _photoService;
        public ProductService(IUnitOfWork unitOfWork, StoreContext context, IPhotoService photoService)
        {
            _context = context;
            _unitOfWork = unitOfWork;
            _photoService = photoService;
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

        public async Task<Product> UpdateProduct(Product product)
        {
            Photo photoToFind = new Photo();

            //Get photos with productId
            var spec = new PhotosWithProductFilterSpecification(product.Id);
            var photos = await _unitOfWork.Repository<Photo>().ListAsync(spec);

            var photosToDelete = photos.Except(product.Photos).ToList();

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

            _unitOfWork.Repository<Product>().Update(product);

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

        public async Task<Product> CreateProduct(Product prod, List<ProductOptions> options)
        {
            var listOptionValue = new List<ProductOptionValues>();

            prod.ProductType = await _unitOfWork.Repository<ProductType>().GetByIdAsync(prod.ProductTypeId);

            _unitOfWork.Repository<Product>().Add(prod);

            var saveOptionResult = await _unitOfWork.Complete();

            if (saveOptionResult <= 0) return null;

            foreach (var option in prod.ProductOptions)
            {
                foreach (var value in option.ProductOptionValues)
                {
                    listOptionValue.Add(value);
                }
            }

            foreach (var sku in prod.ProductSKUs)
            {
                var mainPhoto = prod.Photos.FirstOrDefault(p => p.IsMain == true);

                if(string.IsNullOrEmpty(sku.ImageUrl))
                {
                    sku.ImageUrl = mainPhoto.Url;
                }

                foreach (var value in sku.ProductSKUValues)
                {
                    value.ProductOptionValue = listOptionValue.FirstOrDefault(ov => ov.ValueTempId == value.ValueTempId);
                }
            }

            _unitOfWork.Repository<Product>().Update(prod);

            var result = await _unitOfWork.Complete();

            if (result <= 0) return null;

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