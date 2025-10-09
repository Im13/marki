using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.BackgroundJobs
{
    public class SessionCleanupJob : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<SessionCleanupJob> _logger;

        public SessionCleanupJob(
            IServiceProvider serviceProvider,
            ILogger<SessionCleanupJob> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Session Cleanup Job started");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(TimeSpan.FromDays(7), stoppingToken);

                    using var scope = _serviceProvider.CreateScope();
                    var context = scope.ServiceProvider.GetRequiredService<StoreContext>();

                    await CleanupOldData(context);

                    _logger.LogInformation("Session cleanup completed at {Time}", DateTime.UtcNow);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during session cleanup");
                }
            }
        }

        private async Task CleanupOldData(StoreContext context)
        {
            var interactionCutoff = DateTime.UtcNow.AddDays(-90);
            var sessionCutoff = DateTime.UtcNow.AddDays(-90);
            var trendingCutoff = DateTime.UtcNow.Date.AddDays(-30);

            var deletedInteractions = await context.SessionInteractions
                .Where(i => i.InteractionDate < interactionCutoff)
                .ExecuteDeleteAsync();

            var deletedSessions = await context.UserSessions
                .Where(s => s.LastActivityAt < sessionCutoff)
                .ExecuteDeleteAsync();

            var deletedTrending = await context.ProductTrendings
                .Where(t => t.DateUpdated < trendingCutoff)
                .ExecuteDeleteAsync();

            _logger.LogInformation(
                "Cleanup completed: {Interactions} interactions, {Sessions} sessions, {Trending} trending records deleted",
                deletedInteractions, deletedSessions, deletedTrending);
        }
    }
}