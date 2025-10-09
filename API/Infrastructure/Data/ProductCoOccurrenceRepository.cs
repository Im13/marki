using Core.Entities.Recommendation;
using Core.Interfaces.Recommendations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ProductCoOccurrenceRepository : GenericRepository<ProductCoOccurrence>, IProductCoOccurrenceRepository
    {
        public ProductCoOccurrenceRepository(StoreContext context) : base(context) { }

        public async Task<List<ProductCoOccurrence>> GetByProductIdAsync(int productId, int limit = 10)
        {
            return await _context.ProductCoOccurrences
                .Where(c => c.ProductId1 == productId)
                .OrderByDescending(c => c.CoOccurrenceCount)
                .Take(limit)
                .Include(c => c.Product2)
                .ToListAsync();
        }

        public async Task<Dictionary<int, int>> GetCoOccurrenceDictionaryAsync(int productId, int limit = 10)
        {
            return await _context.ProductCoOccurrences
                .Where(c => c.ProductId1 == productId)
                .OrderByDescending(c => c.CoOccurrenceCount)
                .Take(limit)
                .ToDictionaryAsync(c => c.ProductId2, c => c.CoOccurrenceCount);
        }

        public async Task TruncateAsync()
        {
            await _context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE ProductCoOccurrences");
        }

        public async Task BulkInsertAsync(List<ProductCoOccurrence> coOccurrences)
        {
            await _context.ProductCoOccurrences.AddRangeAsync(coOccurrences);
            await _context.SaveChangesAsync();
        }
    }
}