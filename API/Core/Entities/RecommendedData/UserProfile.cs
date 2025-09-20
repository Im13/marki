namespace Core.Entities.RecommendedData
{
    public class UserProfile
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public List<int> PreferredCategories { get; set; } = new();
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public List<string> PreferredTags { get; set; } = new();
        public DateTime UpdatedAt { get; set; }
    }
}