using Core.DTOs.Recommendations;
using Core.Interfaces.Recommendations;
using Core.Services;

namespace Infrastructure.Services.Recommendations
{
    public class RecommendationService : IRecommendationService
    {
        private readonly IContentBasedRecommender _contentBased;
        private readonly ISessionBasedRecommender _sessionBased;
        private readonly IPopularityBasedRecommender _popularityBased;
        private readonly IRecommendationRepository _recommendationRepo;

        public RecommendationService(
            IContentBasedRecommender contentBased,
            ISessionBasedRecommender sessionBased,
            IPopularityBasedRecommender popularityBased,
            IRecommendationRepository recommendationRepo)
        {
            _contentBased = contentBased;
            _sessionBased = sessionBased;
            _popularityBased = popularityBased;
            _recommendationRepo = recommendationRepo;
        }

        public async Task<List<RecommendationDTO>> GetRecommendationsAsync(
            string sessionId, int limit = 10)
        {
            var allRecommendations = new Dictionary<int, RecommendationDTO>();

            // Session-based (60%)
            var sessionRecs = await _sessionBased.GetRecommendationsAsync(sessionId, limit * 2);
            foreach (var rec in sessionRecs)
            {
                rec.Score *= 0.6;
                allRecommendations[rec.ProductId] = rec;
            }

            // Content-based (30%)
            var contentRecs = await _contentBased.GetRecommendationsAsync(sessionId, limit * 2);
            foreach (var rec in contentRecs)
            {
                rec.Score *= 0.3;
                if (allRecommendations.ContainsKey(rec.ProductId))
                    allRecommendations[rec.ProductId].Score += rec.Score;
                else
                    allRecommendations[rec.ProductId] = rec;
            }

            // Popularity fallback (10%)
            if (allRecommendations.Count < limit / 2)
            {
                var popularRecs = await _popularityBased.GetTrendingProductsAsync(limit);
                foreach (var rec in popularRecs)
                {
                    rec.Score *= 0.1;
                    if (!allRecommendations.ContainsKey(rec.ProductId))
                        allRecommendations[rec.ProductId] = rec;
                }
            }

            return allRecommendations.Values
                .OrderByDescending(r => r.Score)
                .Take(limit)
                .ToList();
        }

        public async Task<List<RecommendationDTO>> GetSimilarProductsAsync(int productId, int limit = 8)
        {
            // Simply delegate to content-based recommender
            return await _contentBased.GetSimilarProductsAsync(productId, limit);
        }

        public Task<List<RecommendationDTO>> GetTrendingProductsAsync(int limit = 10)
        {
            throw new NotImplementedException();
        }
    }
}