using Core.Entities;
using Core.Entities.OrderAggregate;

public interface IRevenueSummaryService
{
    Task<RevenueSummary> GetDailyRevenueAsync(DateTime date);
    Task<IReadOnlyList<RevenueSummary>> GetRevenueInRangeAsync(DateTime startDate, DateTime endDate);
    Task<DashboardRevenueResponse> GetDashboardRevenueDataAsync(DateTime startDate, DateTime endDate);
}