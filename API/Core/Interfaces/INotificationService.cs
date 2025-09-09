namespace Core.Interfaces
{
    public interface INotificationService
    {
        Task CreateNotificationForAdminsAsync(string createdByUserId, string orderId);
    }
}