using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Recommendation;

namespace Core.Interfaces.Recommendations
{
    public interface IProductCoOccurrenceRepository : IGenericRepository<ProductCoOccurrence>
    {
        Task<List<ProductCoOccurrence>> GetByProductIdAsync(int productId, int limit = 10);
        Task<Dictionary<int, int>> GetCoOccurrenceDictionaryAsync(int productId, int limit = 10);
        Task TruncateAsync();
        Task BulkInsertAsync(List<ProductCoOccurrence> coOccurrences);
    }
}