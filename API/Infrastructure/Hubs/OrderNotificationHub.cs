using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace Infrastructure.Hubs
{
    public class OrderNotificationHub : Hub
{
    //Send message to specifict role
    public async Task SendOrderNotification(string role, string message)
    {
        await Clients.Group(role).SendAsync("ReceiveNotification", message);
    }

    public override async Task OnConnectedAsync()
    {
        var user = Context.User;
        if (user?.Identity?.IsAuthenticated == true)
        {
            var roles = user.FindAll(ClaimTypes.Role).Select(r => r.Value);

            foreach (var role in roles)
            {
                if (role == "Admin" || role == "SuperAdmin" || role == "Employee")
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, role);
                }
            }
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
                if (role == "Admin" || role == "SuperAdmin")
                {
                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, role);
                }
            }
        }

        await base.OnDisconnectedAsync(exception);
    }
}
}

