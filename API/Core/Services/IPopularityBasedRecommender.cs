using Core.DTOs.Recommendations;

namespace Core.Services
{
    public interface IPopularityBasedRecommender
    {
        Task<List<RecommendationDTO>> GetTrendingProductsAsync(int limit = 10);
        Task<List<RecommendationDTO>> GetBestSellersAsync(int days = 30, int limit = 10);
    }
}