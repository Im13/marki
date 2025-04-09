namespace API.DTOs.Revenue
{
    public class RevenueSummaryDto
    {
        public DateTime Date { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalOrders { get; set; }
        public decimal ShopeeRevenue { get; set; }
        public decimal FacebookRevenue { get; set; }
        public decimal InstagramRevenue { get; set; }
        public decimal WebsiteRevenue { get; set; }
        public decimal OfflineRevenue { get; set; }
    }
    
    public class DashboardRevenueResponseDTO
    {
        public decimal TotalRevenue { get; set; }
        public int TotalOrders { get; set; }
        public Dictionary<string, decimal> RevenueBySource { get; set; }
        public List<RevenueSummaryDto> DailyRevenues { get; set; }
    }
}