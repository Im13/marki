using Core.Entities.Recommendation;
using Core.Enums;

namespace Core.Interfaces.Recommendations
{
    public interface ISessionInteractionRepository : IGenericRepository<SessionInteraction>
    {
        Task<List<SessionInteraction>> GetBySessionIdAsync(string sessionId);
        Task<List<SessionInteraction>> GetByProductIdAsync(int productId);
        Task<List<SessionInteraction>> GetBySessionAndTypeAsync(string sessionId, InteractionType type);
        Task<Dictionary<int, int>> GetInteractionCountsByProductAsync(DateTime startDate, DateTime endDate);
        Task DeleteOldInteractionsAsync(DateTime cutoffDate);
    }
}