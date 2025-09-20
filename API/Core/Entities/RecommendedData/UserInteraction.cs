using Core.Enums;

namespace Core.Entities.RecommendedData
{
    public class UserInteraction
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public InteractionType Type { get; set; }
        public double Rating { get; set; } // 1-5 stars
        public DateTime Timestamp { get; set; }
        public int Duration { get; set; } // seconds spent viewing
    }
}