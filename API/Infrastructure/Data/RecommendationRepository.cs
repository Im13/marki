using Core.Entities;
using Core.Enums;
using Core.Interfaces.Recommendations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class RecommendationRepository : IRecommendationRepository
    {
        private readonly StoreContext _context;

        public RecommendationRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<List<int>> GetSessionViewedProductsAsync(string sessionId, int limit = 20)
        {
            return await _context.SessionInteractions
                .Where(i => i.SessionId == sessionId && i.InteractionType == InteractionType.View)
                .OrderByDescending(i => i.InteractionDate)
                .Select(i => i.ProductId)
                .Distinct()
                .Take(limit)
                .ToListAsync();
        }

        public async Task<Dictionary<int, int>> GetProductCoOccurrencesAsync(int productId, int limit = 10)
        {
            return await _context.ProductCoOccurrences
                .Where(c => c.ProductId1 == productId)
                .OrderByDescending(c => c.CoOccurrenceCount)
                .Take(limit)
                .ToDictionaryAsync(c => c.ProductId2, c => c.CoOccurrenceCount);
        }

        public Task<List<int>> GetSessionCartProductsAsync(string sessionId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Product>> GetTrendingProductsAsync(int days, int limit = 10)
        {
            throw new NotImplementedException();
        }

        public Task<List<Product>> GetProductsWithStockAsync(List<int> productIds)
        {
            throw new NotImplementedException();
        }
    }
}