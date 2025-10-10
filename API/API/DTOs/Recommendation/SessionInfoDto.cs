namespace API.DTOs.Recommendation
{
    public class SessionInfoDto
    {
        public string SessionId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastActivityAt { get; set; }
        public int InteractionCount { get; set; }
    }
}