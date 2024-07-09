using Core.Entities;
using Core.Specification;

namespace Core.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> GetProductByIdAsync(int id);
        Task<IReadOnlyList<Product>> GetProductsAsync();
        Task<IReadOnlyList<ProductType>> GetProductTypesAsync();
        Task<List<Product>> GetProductsWithSpec(ISpecification<Product> spec);
        
    }
}