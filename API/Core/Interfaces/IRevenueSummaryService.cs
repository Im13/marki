using Core.Entities;

public interface IRevenueSummaryService
{
    Task<RevenueSummary> GetDailyRevenueAsync(DateTime date);
    Task<RevenueSummary> GetRevenueInRangeAsync(DateTime startDate, DateTime endDate);
    Task<DashboardRevenueResponse> GetDashboardRevenueDataAsync(DateTime startDate, DateTime endDate);
}