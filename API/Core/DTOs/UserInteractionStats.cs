using Core.Enums;

namespace API.Core.DTOs
{
    public class UserInteractionStats
    {
        public int TotalInteractions { get; set; }
        public int TotalViews { get; set; }
        public int TotalLikes { get; set; }
        public int TotalCartAdds { get; set; }
        public int TotalPurchases { get; set; }
        public int TotalRatings { get; set; }
        public double AverageRating { get; set; }
        public DateTime? LastInteractionAt { get; set; }
        public Dictionary<string, int> CategoryInteractions { get; set; } = new();
        public Dictionary<InteractionType, int> InteractionsByType { get; set; } = new();

    }
}