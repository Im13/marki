using Core.Entities;

namespace Core.Interfaces
{
    public interface IProductService
    {
        Task<Product> CreateProduct(Product prod);
        Task<Product> GetProductBySKUAsync(string productSKU);
        Task<Product> UpdateProduct(Product product);
    }
}