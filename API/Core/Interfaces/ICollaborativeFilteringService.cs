using Core.Entities.RecommendedData;

namespace Core.Interfaces
{
    public interface ICollaborativeFilteringService
    {
        Task<List<Recommendation>> GetRecommendationsAsync(int appUserId, int count = 10);
        Task<double> PredictRatingAsync(int appUserId, int productId);
        Task TrainModelAsync();
    }
}