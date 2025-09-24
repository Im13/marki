using Core.Entities;
using Core.Entities.RecommendedData;

namespace Core.Interfaces
{
    public interface IContentBasedFilteringService
    {
        Task<List<Recommendation>> GetRecommendationsAsync(int appUserId, int count = 10);
        Task<List<Product>> GetSimilarProductsAsync(int productId, int count = 5);
        Task<double> CalculateSimilarityAsync(int appUserId, int productId);
        Task UpdateProductFeaturesAsync();
    }
}