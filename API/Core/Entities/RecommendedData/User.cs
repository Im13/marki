namespace Core.Entities.RecommendedData
{
    public class User
    {
        public int Id { get; set; } // Same ID as Identity User
        public string DisplayName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? LastActiveAt { get; set; }

        // Navigation properties trong cùng database
        public List<UserInteraction> Interactions { get; set; } = new();
        public UserProfile Profile { get; set; } = new();
    }
}