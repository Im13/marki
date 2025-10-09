using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Recommendation;
using Core.Interfaces.Recommendations;
using Core.Services;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services.Recommendations
{
    public class SessionService : ISessionService
    {
        private readonly ISessionRepository _sessionRepository;

        public SessionService(ISessionRepository sessionRepository)
        {
            _sessionRepository = sessionRepository;
        }

        public async Task<string> GetOrCreateSessionIdAsync(HttpContext httpContext)
        {
            var sessionId = httpContext.Request.Cookies["SessionId"];

            if (string.IsNullOrEmpty(sessionId))
            {
                sessionId = Guid.NewGuid().ToString();

                httpContext.Response.Cookies.Append("SessionId", sessionId, new CookieOptions
                {
                    Expires = DateTimeOffset.Now.AddDays(30),
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None
                });

                var ipAddress = httpContext.Connection.RemoteIpAddress?.ToString();
                var userAgent = httpContext.Request.Headers["User-Agent"].ToString();

                await _sessionRepository.CreateOrUpdateSessionAsync(sessionId, ipAddress, userAgent);
            }
            else
            {
                await UpdateSessionActivityAsync(sessionId);
            }

            return sessionId;
        }

        public async Task<UserSession> GetSessionAsync(string sessionId)
        {
            return await _sessionRepository.GetBySessionIdAsync(sessionId);
        }

        public async Task UpdateSessionActivityAsync(string sessionId)
        {
            var session = await _sessionRepository.GetBySessionIdAsync(sessionId);
            if (session != null)
            {
                session.LastActivityAt = DateTime.UtcNow;
                await _sessionRepository.UpdateAsync(session);
            }
        }
    }
}