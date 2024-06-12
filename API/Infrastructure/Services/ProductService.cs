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
        // public async Task<Product> CreateProduct(Product prod)
        // {
        //     prod.ProductType = await _unitOfWork.Repository<ProductType>().GetByIdAsync(prod.ProductTypeId);
        //     prod.ProductBrand = await _unitOfWork.Repository<ProductBrand>().GetByIdAsync(prod.ProductBrandId);

        //     _unitOfWork.Repository<Product>().Add(prod);

        //     var result = await _unitOfWork.Complete();

        //     if(result <= 0) return null;

        //     return prod;
        // }

        public async Task<Product> GetProductBySKUAsync(string productSKU)
        {
            var products = await _unitOfWork.Repository<Product>().ListAllAsync();

            return products.SingleOrDefault(p => p.ProductSKU == productSKU);
        }

        public async Task<Product> UpdateProduct(Product product) 
        {
            _unitOfWork.Repository<Product>().Update(product);

            var result = await _unitOfWork.Complete();

            if(result <= 0) return null;

            return product;
        }

        public async Task<Product> CreateProduct(Product prod , List<ProductOptions> options)
        {
            List<ProductSKUs> skus = new List<ProductSKUs>();
            prod.ProductType = await _unitOfWork.Repository<ProductType>().GetByIdAsync(prod.ProductTypeId);
            prod.ProductBrand = await _unitOfWork.Repository<ProductBrand>().GetByIdAsync(prod.ProductBrandId);

            foreach(var option in options)
            {
                // var valuesToAdd = new List<ProductOptionValues>();

                // foreach(var value in option.ProductOptionValues)
                // {
                //     _unitOfWork.Repository<ProductOptionValues>().Add(value);
                // }
                _unitOfWork.Repository<ProductOptions>().Add(option);
            }

            var saveOptionResult = await _unitOfWork.Complete();

            // if(saveOptionResult <= 0) return null;

            // // From this line, create products
            // foreach(var sku in prod.ProductSKUs) {
            //     var skuOptions = new List<ProductOptions>();

            //     foreach(var option in sku.ProductSKUValues) {
            //         skuOptions.Add(options.Where(o => o.));
            //     }
            //     skus.Add()
            // }

            // _unitOfWork.Repository<Product>().Add(prod);

            // var result = await _unitOfWork.Complete();

            // if(result <= 0) return null;

            return prod;
        }
    }
}