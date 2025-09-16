using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using Core.Constants;

namespace Infrastructure.Hubs
{
    public class OrderNotificationHub : Hub
{
    //Send message to specific role
    public async Task SendOrderNotification(string role, string message)
    {
        if (string.IsNullOrWhiteSpace(role)) return;
        // Only allow known roles
        if (!RoleConstants.ALL_ROLES.Contains(role)) return;
        await Clients.Group(role).SendAsync("ReceiveNotification", message);
    }

    //Send notification to all admin groups
    public async Task SendNotificationToAdmins(object notificationData)
    {
        await Clients.Groups(RoleConstants.ORDER_NOTIFICATION_ROLES).SendAsync("ReceiveNotification", notificationData);
    }

    //Join specific group
    public async Task JoinGroup(string groupName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
    }

    //Leave specific group
    public async Task LeaveGroup(string groupName)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
    }

    public override async Task OnConnectedAsync()
    {
        var user = Context.User;
        Console.WriteLine($"SignalR OnConnected - User authenticated: {user?.Identity?.IsAuthenticated}");
        Console.WriteLine($"SignalR OnConnected - User email: {user?.FindFirst(ClaimTypes.Email)?.Value}");
        
        if (user?.Identity?.IsAuthenticated == true)
        {
            var roles = user.FindAll(ClaimTypes.Role).Select(r => r.Value);
            var joinableRoles = new[] { RoleConstants.ADMIN, RoleConstants.SUPER_ADMIN, RoleConstants.EMPLOYEE };
            
            Console.WriteLine($"SignalR OnConnected - User roles: {string.Join(", ", roles)}");

            foreach (var role in roles)
            {
                if (joinableRoles.Contains(role))
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, role);
                    Console.WriteLine($"SignalR OnConnected - Added to group: {role}");
                }
            }
        }
        else
        {
            Console.WriteLine("SignalR OnConnected - User not authenticated, closing connection");
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
        }

        await base.OnDisconnectedAsync(exception);
    }
}
}

