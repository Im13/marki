using Core.Entities;

public interface IRevenueSummaryService
{
    Task<RevenueSummary> GetDailyRevenueAsync(DateTime date);
    Task<RevenueSummary> GetRevenueInRangeAsync(DateTime startDate, DateTime endDate);
    Task<IReadOnlyList<RevenueSummary>> GetDashboardRevenueDataAsync(DateTime startDate, DateTime endDate);
}