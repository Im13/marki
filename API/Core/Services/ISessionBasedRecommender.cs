using Core.DTOs.Recommendations;

namespace Core.Services
{
    public interface ISessionBasedRecommender
    {
        Task<List<RecommendationDTO>> GetRecommendationsAsync(string sessionId, int limit = 10);
    }
}