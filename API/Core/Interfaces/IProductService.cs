using Core.Entities;

namespace Core.Interfaces
{
    public interface IProductService
    {
        Task<Product> CreateProduct(Product prod, List<ProductOptions> options);
        Task<Product> GetProductBySKUAsync(string productSKU);
        Task<Product> UpdateProduct(Product product);
        Task<Product> GetProductAsync(int id);
        Task<bool> DeleteProducts(List<Product> products);
    }
}