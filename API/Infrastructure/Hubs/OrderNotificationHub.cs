using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using Core.Constants;

namespace Infrastructure.Hubs
{
    public class OrderNotificationHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var user = Context.User;

            if (user?.Identity?.IsAuthenticated == true)
            {
                var roles = user.FindAll(ClaimTypes.Role).Select(r => r.Value);
                var joinableRoles = new[] { RoleConstants.ADMIN, RoleConstants.SUPER_ADMIN, RoleConstants.EMPLOYEE };

                foreach (var role in roles)
                {
                    if (joinableRoles.Contains(role))
                    {
                        await Groups.AddToGroupAsync(Context.ConnectionId, role);
                    }
                }

                // Add to order notifications group if user has admin roles
                var orderNotificationRoles = roles.Where(r => RoleConstants.ORDER_NOTIFICATION_ROLES.Contains(r));
                if (orderNotificationRoles.Any())
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, "OrderNotifications");
                }
            }
            else
            {
                Context.Abort();
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var user = Context.User;
            if (user?.Identity?.IsAuthenticated == true)
            {
                var roles = user.FindAll(ClaimTypes.Role).Select(c => c.Value);

                foreach (var role in roles)
                {
                    if (role == RoleConstants.ADMIN || role == RoleConstants.SUPER_ADMIN)
                    {
                        await Groups.RemoveFromGroupAsync(Context.ConnectionId, role);
                    }
                }

                // Remove from order notifications group
                var orderNotificationRoles = roles.Where(r => RoleConstants.ORDER_NOTIFICATION_ROLES.Contains(r));
                if (orderNotificationRoles.Any())
                {
                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, "OrderNotifications");
                }
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}

