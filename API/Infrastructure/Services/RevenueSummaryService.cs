using Core.Entities;
using Core.Interfaces;

public class RevenueSummaryService : IRevenueSummaryService
{
    private readonly IRevenueSummaryRepository _revenueRepository;

    public RevenueSummaryService(IRevenueSummaryRepository revenueRepository)
    {
        _revenueRepository = revenueRepository;
    }

    public async Task<RevenueSummary> GetDailyRevenueAsync(DateTime date)
    {
        return await _revenueRepository.GetByDateAsync(date);
    }

    public async Task<DashboardRevenueResponse> GetDashboardRevenueDataAsync(DateTime startDate, DateTime endDate)
    {
        var revenues = await _revenueRepository.GetByDateRangeAsync(startDate, endDate);
        
        return new DashboardRevenueResponse
        {
            TotalRevenue = revenues.Sum(r => r.TotalRevenue),
            TotalOrders = revenues.Sum(r => r.TotalOrders),
            RevenueBySource = new Dictionary<string, decimal>
            {
                { "Shopee", revenues.Sum(r => r.ShopeeRevenue) },
                { "Facebook", revenues.Sum(r => r.FacebookRevenue) },
                { "Instagram", revenues.Sum(r => r.InstagramRevenue) },
                { "Offline", revenues.Sum(r => r.OfflineRevenue) },
                { "Website", revenues.Sum(r => r.WebsiteRevenue) }
            },
            DailyRevenues = revenues.ToList()
        };
    }

    public async Task<RevenueSummary> GetRevenueInRangeAsync(DateTime startDate, DateTime endDate)
    {
        var revenues =  await _revenueRepository.GetByDateRangeAsync(startDate, endDate);

        return new RevenueSummary
        {
            TotalRevenue = revenues.Sum(r => r.TotalRevenue),
            TotalOrders = revenues.Sum(r => r.TotalOrders),
            ShopeeRevenue = revenues.Sum(r => r.ShopeeRevenue),
            FacebookRevenue = revenues.Sum(r => r.FacebookRevenue),
            InstagramRevenue = revenues.Sum(r => r.InstagramRevenue),
            WebsiteRevenue = revenues.Sum(r => r.WebsiteRevenue),
            OfflineRevenue = revenues.Sum(r => r.OfflineRevenue)
        };
    }
}