using Core.Entities;
using Core.Entities.RecommendedData;

namespace Core.Interfaces
{
    public interface IRecommendationRepository
    {
        Task<List<Recommendation>> GetUserRecommendationsAsync(int appUserId, int count = 10);
        Task<List<Product>> GetSimilarProductsAsync(int productId, int count = 5);
        Task SaveRecommendationsAsync(List<Recommendation> recommendations);
        Task<List<UserInteraction>> GetUserInteractionsAsync(int appUserId);
        Task<List<UserInteraction>> GetAllInteractionsAsync();
        Task<List<Product>> GetProductsByIdsAsync(List<int> productIds);
        Task<List<Product>> GetAllProductsAsync();
    }
}