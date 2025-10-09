using Core.Entities.Recommendation;
using Core.Enums;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.BackgroundJobs
{
    public class TrendingUpdateJob : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<TrendingUpdateJob> _logger;

        public TrendingUpdateJob(
            IServiceProvider serviceProvider,
            ILogger<TrendingUpdateJob> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Trending Update Job started");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(TimeSpan.FromHours(1), stoppingToken);

                    using var scope = _serviceProvider.CreateScope();
                    var context = scope.ServiceProvider.GetRequiredService<StoreContext>();

                    await UpdateTrending(context);

                    _logger.LogInformation("Trending products updated successfully at {Time}", DateTime.UtcNow);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating trending products");
                }
            }
        }

        private async Task UpdateTrending(StoreContext context)
        {
            var today = DateTime.UtcNow.Date;
            var startOfDay = today;
            var endOfDay = today.AddDays(1);

            var todayStats = await context.SessionInteractions
                .Where(i => i.InteractionDate >= startOfDay && i.InteractionDate < endOfDay)
                .GroupBy(i => new { i.ProductId, i.InteractionType })
                .Select(g => new
                {
                    ProductId = g.Key.ProductId,
                    InteractionType = g.Key.InteractionType,
                    Count = g.Count()
                })
                .ToListAsync();

            foreach (var productGroup in todayStats.GroupBy(s => s.ProductId))
            {
                var productId = productGroup.Key;
                var viewCount = productGroup.FirstOrDefault(g => g.InteractionType == InteractionType.View)?.Count ?? 0;
                var cartCount = productGroup.FirstOrDefault(g => g.InteractionType == InteractionType.AddToCart)?.Count ?? 0;
                var purchaseCount = productGroup.FirstOrDefault(g => g.InteractionType == InteractionType.Purchase)?.Count ?? 0;

                var trendingScore = (purchaseCount * 10) + (cartCount * 3) + (viewCount * 1.0m);

                var existingRecord = await context.ProductTrendings
                    .FirstOrDefaultAsync(t => t.ProductId == productId && t.DateUpdated == today);

                if (existingRecord != null)
                {
                    existingRecord.ViewCount = viewCount;
                    existingRecord.CartCount = cartCount;
                    existingRecord.PurchaseCount = purchaseCount;
                    existingRecord.TrendingScore = trendingScore;
                }
                else
                {
                    await context.ProductTrendings.AddAsync(new ProductTrending
                    {
                        ProductId = productId,
                        ViewCount = viewCount,
                        CartCount = cartCount,
                        PurchaseCount = purchaseCount,
                        TrendingScore = trendingScore,
                        DateUpdated = today
                    });
                }
            }

            await context.SaveChangesAsync();

            var topTrending = await context.ProductTrendings
                .Where(t => t.DateUpdated >= today.AddDays(-7))
                .GroupBy(t => t.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    TotalScore = g.Sum(t => t.TrendingScore)
                })
                .OrderByDescending(x => x.TotalScore)
                .Take(20)
                .Select(x => x.ProductId)
                .ToListAsync();

            await context.Products
                .ExecuteUpdateAsync(p => p.SetProperty(x => x.IsTrending, false));

            await context.Products
                .Where(p => topTrending.Contains(p.Id))
                .ExecuteUpdateAsync(p => p.SetProperty(x => x.IsTrending, true));

            _logger.LogInformation("Marked {Count} products as trending", topTrending.Count);
        }
    }
}