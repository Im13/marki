using Core.Entities.Recommendation;
using Microsoft.AspNetCore.Http;

namespace Core.Services
{
    public interface ISessionService
    {
        Task<string> GetOrCreateSessionIdAsync(HttpContext httpContext);
        Task<UserSession> GetSessionAsync(string sessionId);
        Task UpdateSessionActivityAsync(string sessionId);
    }
}