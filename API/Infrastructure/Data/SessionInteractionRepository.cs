using Core.Entities.Recommendation;
using Core.Enums;
using Core.Interfaces.Recommendations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class SessionInteractionRepository : GenericRepository<SessionInteraction>, ISessionInteractionRepository
    {
        public SessionInteractionRepository(StoreContext context) : base(context) { }

        public async Task<List<SessionInteraction>> GetBySessionIdAsync(string sessionId)
        {
            return await _context.SessionInteractions
                .Where(i => i.SessionId == sessionId)
                .Include(i => i.Product)
                    .ThenInclude(p => p.ProductSKUs)
                .Include(i => i.Product)
                    .ThenInclude(p => p.ProductOptions)
                        .ThenInclude(po => po.ProductOptionValues)
                .OrderByDescending(i => i.InteractionDate)
                .ToListAsync();
        }

        public async Task<List<SessionInteraction>> GetByProductIdAsync(int productId)
        {
            return await _context.SessionInteractions
                .Where(i => i.ProductId == productId)
                .OrderByDescending(i => i.InteractionDate)
                .ToListAsync();
        }

        public async Task<List<SessionInteraction>> GetBySessionAndTypeAsync(string sessionId, InteractionType type)
        {
            return await _context.SessionInteractions
                .Where(i => i.SessionId == sessionId && i.InteractionType == type)
                .ToListAsync();
        }

        public async Task<Dictionary<int, int>> GetInteractionCountsByProductAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.SessionInteractions
                .Where(i => i.InteractionDate >= startDate && i.InteractionDate < endDate)
                .GroupBy(i => i.ProductId)
                .Select(g => new { ProductId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.ProductId, x => x.Count);
        }

        public async Task DeleteOldInteractionsAsync(DateTime cutoffDate)
        {
            await _context.SessionInteractions
                .Where(i => i.InteractionDate < cutoffDate)
                .ExecuteDeleteAsync();
        }

    }
}