using Core.Entities;

namespace Core.Interfaces
{
    public interface IProductService
    {
        Task<Product> CreateProduct(Product prod);
    }
}