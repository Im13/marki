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

    public async Task<IReadOnlyList<RevenueSummary>> GetDashboardRevenueDataAsync(DateTime startDate, DateTime endDate)
    {
        var revenues = await _revenueRepository.GetByDateRangeAsync(startDate, endDate);

        // Tạo danh sách tất cả các ngày trong khoảng thời gian
        var allDates = Enumerable.Range(0, (endDate - startDate).Days + 1)
            .Select(offset => startDate.AddDays(offset))
            .ToList();

        // Tạo danh sách kết quả, đảm bảo mỗi ngày đều có dữ liệu
        var completeRevenues = allDates.Select(date =>
        {
            // Tìm RevenueSummary cho ngày hiện tại
            var revenueForDate = revenues.FirstOrDefault(r => r.Date.Date == date.Date);

            // Nếu không có dữ liệu, tạo đối tượng mặc định
            return revenueForDate ?? new RevenueSummary
            {
                Date = date,
                TotalRevenue = 0,
                TotalOrders = 0,
                ShopeeRevenue = 0,
                FacebookRevenue = 0,
                InstagramRevenue = 0,
                WebsiteRevenue = 0,
                OfflineRevenue = 0
            };
        }).ToList();

        return completeRevenues;
    }

    public async Task<RevenueSummary> GetRevenueInRangeAsync(DateTime startDate, DateTime endDate)
    {
        var revenues = await _revenueRepository.GetByDateRangeAsync(startDate, endDate);

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