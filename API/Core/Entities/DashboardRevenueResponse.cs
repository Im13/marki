namespace Core.Entities
{
    public class DashboardRevenueResponse
    {
        public decimal TotalRevenue { get; set; }
        public int TotalOrders { get; set; }
        public Dictionary<string, decimal> RevenueBySource { get; set; }
        public List<RevenueSummary> DailyRevenues { get; set; }
    }
}