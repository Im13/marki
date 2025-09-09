using Core.Entities.Identity;

namespace Core.Entities
{
    public class Notification
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string CreatedByUserId { get; set; }
        public ICollection<NotificationUser> NotificationUsers { get; set; } = new List<NotificationUser>();
    }

    public class NotificationUser
    {
        public Guid Id { get; set; }
        public Guid NotificationId { get; set; }
        public int UserId { get; set; }
        public bool IsRead { get; set; } = false;
        public Notification? Notification { get; set; }
    }
}