using Core.Enums;

namespace Core.Entities.RecommendedData
{
    public class Recommendation
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public double Score { get; set; }
        public RecommendationType Type { get; set; }
        public string Reason { get; set; } = string.Empty;
        public DateTime GeneratedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}