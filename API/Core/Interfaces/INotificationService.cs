namespace Core.Interfaces
{
    public interface INotificationService
    {
        Task CreateNotificationForAdminsAsync(string createdByUserId, string orderId);
        Task<IReadOnlyList<Core.DTOs.NotificationDTO>> GetMyNotificationsAsync(int userId, int take = 50);
        Task MarkAsReadAsync(Guid notificationId, int userId);
    }
}