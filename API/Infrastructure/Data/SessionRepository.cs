using Core.Entities.Recommendation;
using Core.Interfaces.Recommendations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class SessionRepository : GenericRepository<UserSession>, ISessionRepository
    {
        public SessionRepository(StoreContext context) : base(context) { }

        public async Task<UserSession> GetBySessionIdAsync(string sessionId)
        {
            return await _context.UserSessions
                .Include(s => s.Interactions)
                .FirstOrDefaultAsync(s => s.SessionId == sessionId);
        }

        public async Task<UserSession> CreateOrUpdateSessionAsync(string sessionId, string ipAddress, string userAgent)
        {
            var session = await _context.UserSessions
                .FirstOrDefaultAsync(s => s.SessionId == sessionId);

            if (session == null)
            {
                session = new UserSession
                {
                    SessionId = sessionId,
                    CreatedAt = DateTime.UtcNow,
                    LastActivityAt = DateTime.UtcNow,
                    IpAddress = ipAddress,
                    UserAgent = userAgent
                };
                await _context.UserSessions.AddAsync(session);
            }
            else
            {
                session.LastActivityAt = DateTime.UtcNow;
                _context.UserSessions.Update(session);
            }

            await _context.SaveChangesAsync();
            return session;
        }

        public async Task<List<UserSession>> GetInactiveSessionsAsync(DateTime cutoffDate)
        {
            return await _context.UserSessions
                .Where(s => s.LastActivityAt < cutoffDate)
                .ToListAsync();
        }
    }
}