using Core.Entities;
using Core.Entities.Identity;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Hubs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using API.Core.Constants;

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
            // 1. Create Notification
            var notification = new Notification
            {
                Title = "New Order",
                Message = $"Order #{orderId} has been placed.",
                CreatedAt = DateTime.UtcNow,
                CreatedByUserId = createdByUserId
            };

            _storeContext.Notifications.Add(notification);
            await _storeContext.SaveChangesAsync();

            // 2. Collect target users by roles (deduplicated)
            var uniqueUserIds = new HashSet<int>();
            var notificationUsers = new List<NotificationUser>();

            foreach (var role in RoleConstants.ORDER_NOTIFICATION_ROLES)
            {
                var usersInRole = await _userManager.GetUsersInRoleAsync(role);
                foreach (var user in usersInRole)
                {
                    if (uniqueUserIds.Add(user.Id))
                    {
                        notificationUsers.Add(new NotificationUser
                        {
                            NotificationId = notification.Id,
                            UserId = user.Id
                        });
                    }
                }
            }

            if (notificationUsers.Count > 0)
            {
                _storeContext.NotificationUsers.AddRange(notificationUsers);
            }
            await _storeContext.SaveChangesAsync();

            // Send notification to groups
            await _hubContext.Clients.Groups(RoleConstants.ORDER_NOTIFICATION_ROLES).SendAsync("ReceiveNotification", notification);
        }
    }
}