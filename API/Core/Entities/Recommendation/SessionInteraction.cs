using Core.Enums;

namespace Core.Entities.Recommendation
{
    public class SessionInteraction : BaseEntity
    {
        public string SessionId { get; set; }
        public int ProductId { get; set; }
        public InteractionType InteractionType { get; set; }
        public DateTime InteractionDate { get; set; }
        public int? DurationSeconds { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public Product Product { get; set; }
        public UserSession Session { get; set; }
    }
}