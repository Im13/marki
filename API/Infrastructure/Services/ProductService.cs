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
        public async Task<Product> CreateProduct(Product prod)
        {
            prod.ProductType = await _unitOfWork.Repository<ProductType>().GetByIdAsync(prod.ProductTypeId);
            prod.ProductBrand = await _unitOfWork.Repository<ProductBrand>().GetByIdAsync(prod.ProductBrandId);

            _unitOfWork.Repository<Product>().Add(prod);

            var result = await _unitOfWork.Complete();

            if(result <= 0) return null;

            return prod;
        }
    }
}