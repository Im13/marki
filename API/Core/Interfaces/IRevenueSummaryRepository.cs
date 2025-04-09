using Core.Entities;
using Core.Entities.OrderAggregate;

namespace Core.Interfaces
{
    public interface IRevenueSummaryRepository
    {
        Task<RevenueSummary> GetByDateAsync(DateTime date);
        Task<IReadOnlyList<RevenueSummary>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task UpdateRevenueAsync(Order order); 
        Task UpdateDailyRevenueAsync(Order order); 

    }
}