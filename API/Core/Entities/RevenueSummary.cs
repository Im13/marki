namespace Core.Entities
{
    public class RevenueSummary : BaseEntity
    {
        public DateTime Date { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalOrders { get; set; }
        public decimal ShopeeRevenue { get; set; }
        public decimal FacebookRevenue { get; set; }
        public decimal InstagramRevenue { get; set; }
        public decimal WebsiteRevenue { get; set; }
    }
}

