using Core.Entities;
using Core.Entities.Identity;
using Core.Interfaces;
using Core.DTOs;
using Core.Constants;
using Infrastructure.Data;
using Infrastructure.Hubs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly StoreContext _storeContext;
        private readonly IHubContext<OrderNotificationHub> _hubContext;
        private readonly UserManager<AppUser> _userManager;
        private static readonly TimeZoneInfo VietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Ho_Chi_Minh");

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
                Title = "Đơn hàng mới",
                Message = $"Đơn hàng #{orderId} đã được đặt.",
                CreatedAt = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, VietnamTimeZone),
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

            // Create DTO for SignalR
            var notificationDto = new NotificationDTO
            {
                Id = notification.Id,
                Title = notification.Title,
                Message = notification.Message,
                CreatedAt = notification.CreatedAt,
                CreatedByUserId = notification.CreatedByUserId
            };

            // Send notification to order notification group (single group to avoid duplicates)
            await _hubContext.Clients.Groups("OrderNotifications").SendAsync("NewOrderCreated", notificationDto);
        }

        public async Task<IReadOnlyList<NotificationDTO>> GetMyNotificationsAsync(int userId, int take = 50)
        {
            var query = from nu in _storeContext.NotificationUsers.AsNoTracking()
                        join n in _storeContext.Notifications.AsNoTracking() on nu.NotificationId equals n.Id
                        where nu.UserId == userId
                        orderby n.CreatedAt descending
                        select new NotificationDTO
                        {
                            Id = n.Id,
                            Title = n.Title,
                            Message = n.Message,
                            CreatedAt = n.CreatedAt,
                            CreatedByUserId = n.CreatedByUserId,
                            IsRead = nu.IsRead
                        };

            return await query.Take(take).ToListAsync();
        }

        public async Task MarkAsReadAsync(Guid notificationId, int userId)
        {
            var nu = await _storeContext.NotificationUsers
                .FirstOrDefaultAsync(x => x.NotificationId == notificationId && x.UserId == userId);
            if (nu == null) return;

            if (!nu.IsRead)
            {
                nu.IsRead = true;
                await _storeContext.SaveChangesAsync();
            }
        }
    }
}