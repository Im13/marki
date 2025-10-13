using Core.Entities;

namespace Core.Interfaces
{
    public interface IProductService
    {
        Task<Product> CreateProduct(Product prod);
        Task<Product> GetProductBySKUAsync(string productSKU);
        Task<Product> UpdateProduct(Product product);
        Task<Product> GetProductAsync(int id);
        Task<bool> DeleteProducts(List<Product> products);
        Task<ProductSKUs> GetProductSKU(int skuId);
        Task<Product> GetBySlug(string slug);
    }
}