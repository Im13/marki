using Core.Entities.Recommendation;
using Core.Interfaces.Recommendations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ProductTrendingRepository : GenericRepository<ProductTrending>, IProductTrendingRepository
    {
        public ProductTrendingRepository(StoreContext context) : base(context) { }

        public async Task<List<ProductTrending>> GetTrendingByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.ProductTrendings
                .Where(t => t.DateUpdated >= startDate && t.DateUpdated < endDate)
                .Include(t => t.Product)
                .ToListAsync();
        }

        public async Task<ProductTrending> GetByProductAndDateAsync(int productId, DateTime date)
        {
            return await _context.ProductTrendings
                .FirstOrDefaultAsync(t => t.ProductId == productId && t.DateUpdated == date.Date);
        }

        public async Task<List<ProductTrending>> GetTopTrendingAsync(int days, int limit = 10)
        {
            var cutoffDate = DateTime.UtcNow.Date.AddDays(-days);

            return await _context.ProductTrendings
                .Where(t => t.DateUpdated >= cutoffDate)
                .GroupBy(t => t.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    TotalScore = g.Sum(t => t.TrendingScore)
                })
                .OrderByDescending(x => x.TotalScore)
                .Take(limit)
                .Join(_context.Products.Include(p => p.ProductSKUs).Include(p => p.Photos),
                      trending => trending.ProductId,
                      product => product.Id,
                      (trending, product) => new ProductTrending
                      {
                          ProductId = product.Id,
                          Product = product,
                          TrendingScore = trending.TotalScore
                      })
                .ToListAsync();
        }

        public async Task UpdateOrCreateAsync(ProductTrending trending)
        {
            var existing = await GetByProductAndDateAsync(trending.ProductId, trending.DateUpdated);

            if (existing != null)
            {
                existing.ViewCount = trending.ViewCount;
                existing.CartCount = trending.CartCount;
                existing.PurchaseCount = trending.PurchaseCount;
                existing.TrendingScore = trending.TrendingScore;
                _context.ProductTrendings.Update(existing);
            }
            else
            {
                await _context.ProductTrendings.AddAsync(trending);
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteOldRecordsAsync(DateTime cutoffDate)
        {
            await _context.ProductTrendings
                .Where(t => t.DateUpdated < cutoffDate)
                .ExecuteDeleteAsync();
        }
    }
}