namespace Core.Entities.Recommendation
{
    public class ProductTrending : BaseEntity
    {
        public int ProductId { get; set; }
        public int ViewCount { get; set; }
        public int CartCount { get; set; }
        public int PurchaseCount { get; set; }
        public decimal TrendingScore { get; set; }
        public DateTime DateUpdated { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public Product Product { get; set; }
    }
}