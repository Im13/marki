using Core.Entities;
using Core.Entities.Identity;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Extensions;
using Infrastructure.Hubs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly StoreContext _storeContext;
        private readonly IHubContext<OrderNotificationHub> _hubContext;
        private readonly UserManager<AppUser> _userManager;

        public NotificationService(StoreContext storeContext, IHubContext<OrderNotificationHub> hubContext, UserManager<AppUser> userManager)
        {
            _storeContext = storeContext;
            _hubContext = hubContext;
            _userManager = userManager;
        }
        public async Task CreateNotificationForAdminsAsync(string createdByUserId, string orderId)
        {
            // 1. Tạo Notification
            var notification = new Notification
            {
                Title = "New Order",
                Message = $"Order {orderId} has been placed.",
                CreatedAt = DateTime.UtcNow,
                CreatedByUserId = createdByUserId
            };

            _storeContext.Notifications.Add(notification);
            await _storeContext.SaveChangesAsync();

            // Lấy tất cả Admin và SuperAdmin
            var admins = await _userManager.GetUsersInRolesAsync("Admin");
            var superAdmins = await _userManager.GetUsersInRoleAsync("SuperAdmin");

            var targetUsers = admins.Concat(superAdmins).ToList();

            foreach (var user in targetUsers)
            {
                _storeContext.NotificationUsers.Add(new NotificationUser
                {
                    NotificationId = notification.Id,
                    UserId = user.Id
                });
            }
            await _storeContext.SaveChangesAsync();

            await _hubContext.Clients.Groups("Admin", "SuperAdmin")
                .SendAsync("ReceiveNotification", notification);
        }
    }
}