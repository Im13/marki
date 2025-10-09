namespace Core.Entities.Recommendation
{
    public class UserSession : BaseEntity
    {
        public string SessionId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime LastActivityAt { get; set; }
        public string IpAddress { get; set; }
        public string UserAgent { get; set; }
        public ICollection<SessionInteraction> Interactions { get; set; } = new List<SessionInteraction>();
    }
}