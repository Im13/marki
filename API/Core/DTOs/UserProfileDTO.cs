namespace API.Core.DTOs
{
    public class UserProfileDTO
    {
        public int UserId { get; set; }
        public List<int> PreferredCategories { get; set; } = new();
        public List<string> PreferredCategoryNames { get; set; } = new();
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public List<string> PreferredTags { get; set; } = new();
        public DateTime UpdatedAt { get; set; }
    }
}