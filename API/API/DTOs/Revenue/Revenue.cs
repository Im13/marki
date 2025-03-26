namespace API.DTOs.Revenue
{
    public class RevenueSummaryDto
    {
        public DateTime Date { get; set; }
        public decimal DailyRevenue { get; set; }
        public int OrderCount { get; set; }
        public decimal AverageOrderValue { get; set; }
        public string TopSellingCategory { get; set; }
        public int ProductsSold { get; set; }
    }
    
    public class DashboardRevenueResponseDTO
    {
        public decimal TotalRevenue { get; set; }
        public int TotalOrders { get; set; }
        public Dictionary<string, decimal> RevenueBySource { get; set; }
        public List<RevenueSummaryDto> DailyRevenues { get; set; }
    }
}