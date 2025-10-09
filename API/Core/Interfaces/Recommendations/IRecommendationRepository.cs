using Core.Entities;

namespace Core.Interfaces.Recommendations
{
    public interface IRecommendationRepository
    {
        Task<List<int>> GetSessionViewedProductsAsync(string sessionId, int limit = 20);
        Task<List<int>> GetSessionCartProductsAsync(string sessionId);
        Task<Dictionary<int, int>> GetProductCoOccurrencesAsync(int productId, int limit = 10);
        Task<List<Product>> GetTrendingProductsAsync(int days, int limit = 10);
        Task<List<Product>> GetProductsWithStockAsync(List<int> productIds);
    }
}