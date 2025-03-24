using Core.Entities.OrderAggregate;

public interface IRevenueSummaryService
{
    Task UpdateRevenueSummaryFromOrder(Order order);
    // Task<Dictionary<DateTime, decimal>> GetDailyRevenue(DateTime startDate, DateTime endDate);
    // Task<Dictionary<string, decimal>> GetRevenueBySource(DateTime startDate, DateTime endDate);
    // Thêm các method khác theo nhu cầu
}