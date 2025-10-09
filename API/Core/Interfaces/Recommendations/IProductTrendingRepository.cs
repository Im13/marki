using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Recommendation;

namespace Core.Interfaces.Recommendations
{
    public interface IProductTrendingRepository : IGenericRepository<ProductTrending>
    {
        Task<List<ProductTrending>> GetTrendingByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<ProductTrending> GetByProductAndDateAsync(int productId, DateTime date);
        Task<List<ProductTrending>> GetTopTrendingAsync(int days, int limit = 10);
        Task UpdateOrCreateAsync(ProductTrending trending);
        Task DeleteOldRecordsAsync(DateTime cutoffDate);
    }
}