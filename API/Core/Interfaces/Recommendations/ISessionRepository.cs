using Core.Entities.Recommendation;

namespace Core.Interfaces.Recommendations
{
    public interface ISessionRepository : IGenericRepository<UserSession>
    {
        Task<UserSession> GetBySessionIdAsync(string sessionId);
        Task<UserSession> CreateOrUpdateSessionAsync(string sessionId, string ipAddress, string userAgent);
    }
}