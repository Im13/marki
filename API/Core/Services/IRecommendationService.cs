using Core.DTOs.Recommendations;

namespace Core.Services
{
    public interface IRecommendationService
    {
        Task<List<RecommendationDTO>> GetRecommendationsAsync(string sessionId, int limit = 10);
        Task<List<RecommendationDTO>> GetTrendingProductsAsync(int limit = 10);
        Task<List<RecommendationDTO>> GetSimilarProductsAsync(int productId, int limit = 8);
    }
}