using Core.Entities.Recommendation;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.BackgroundJobs
{
    public class CoOccurrenceUpdateJob : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<CoOccurrenceUpdateJob> _logger;

        public CoOccurrenceUpdateJob(
            IServiceProvider serviceProvider,
            ILogger<CoOccurrenceUpdateJob> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(TimeSpan.FromHours(24), stoppingToken);

                    using var scope = _serviceProvider.CreateScope();
                    var context = scope.ServiceProvider.GetRequiredService<StoreContext>();

                    await UpdateCoOccurrences(context);

                    _logger.LogInformation("Co-occurrence matrix updated successfully");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating co-occurrence matrix");
                }
            }
        }

        private async Task UpdateCoOccurrences(StoreContext context)
        {
            var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);

            var sessionProducts = await context.SessionInteractions
                .Where(i => i.InteractionDate >= thirtyDaysAgo)
                .GroupBy(i => i.SessionId)
                .Select(g => g.Select(i => i.ProductId).Distinct().ToList())
                .ToListAsync();

            var coOccurrences = new Dictionary<(int, int), int>();

            foreach (var products in sessionProducts)
            {
                for (int i = 0; i < products.Count; i++)
                {
                    for (int j = i + 1; j < products.Count; j++)
                    {
                        var key = products[i] < products[j]
                            ? (products[i], products[j])
                            : (products[j], products[i]);

                        if (coOccurrences.ContainsKey(key))
                            coOccurrences[key]++;
                        else
                            coOccurrences[key] = 1;
                    }
                }
            }

            await context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE ProductCoOccurrences");

            var newCoOccurrences = coOccurrences
                .Where(kvp => kvp.Value >= 2)
                .SelectMany(kvp => new[]
                {
                    new ProductCoOccurrence
                    {
                        ProductId1 = kvp.Key.Item1,
                        ProductId2 = kvp.Key.Item2,
                        CoOccurrenceCount = kvp.Value,
                        LastUpdated = DateTime.UtcNow
                    },
                    new ProductCoOccurrence
                    {
                        ProductId1 = kvp.Key.Item2,
                        ProductId2 = kvp.Key.Item1,
                        CoOccurrenceCount = kvp.Value,
                        LastUpdated = DateTime.UtcNow
                    }
                })
                .ToList();

            await context.ProductCoOccurrences.AddRangeAsync(newCoOccurrences);
            await context.SaveChangesAsync();

            _logger.LogInformation("Updated {Count} co-occurrence pairs", newCoOccurrences.Count);
        }
    }
}