using Core;
using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
            _unitOfWork.Repository<Product>().Update(product);
            foreach(var sku in product.ProductSKUs)
            {
                foreach(var skuValue in sku.ProductSKUValues)
                {
                    _unitOfWork.Repository<ProductSKUValues>().Update(skuValue);
                }
                
                _unitOfWork.Repository<ProductSKUs>().Update(sku);
            }

            var result = await _unitOfWork.Complete();

            if (result <= 0) return null;

            return product;
        }

        public async Task<Product> CreateProduct(Product prod, List<ProductOptions> options)
        {
            var listOptionValue = new List<ProductOptionValues>();
            prod.ProductType = await _unitOfWork.Repository<ProductType>().GetByIdAsync(prod.ProductTypeId);
            prod.ProductBrand = await _unitOfWork.Repository<ProductBrand>().GetByIdAsync(prod.ProductBrandId);

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
            if(products.Count == 0) return false;

            foreach(var product in products)
            {
                if(product.Id == 0) return false;
                
                product.IsDeleted = true;
                _unitOfWork.Repository<Product>().Update(product);
            }

            var result = await _unitOfWork.Complete();

            if(result <= 0) return false;

            return true;
        }
    }
}