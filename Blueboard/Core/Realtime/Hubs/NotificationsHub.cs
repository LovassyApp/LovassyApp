using Blueboard.Core.Auth;
using Blueboard.Core.Realtime.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Blueboard.Core.Realtime.Hubs;

[Authorize]
public class NotificationsHub : Hub<INotificationsClient>
{
    public override async Task OnConnectedAsync()
    {
        var permissions = Context.User!.Claims.Where(c => c.Type == AuthConstants.PermissionClaim).ToList();

        foreach (var permission in permissions) await Groups.AddToGroupAsync(Context.ConnectionId, permission.Value);

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var permissions = Context.User!.Claims.Where(c => c.Type == AuthConstants.PermissionClaim).ToList();

        foreach (var permission in permissions)
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, permission.Value);

        await base.OnDisconnectedAsync(exception);
    }
}