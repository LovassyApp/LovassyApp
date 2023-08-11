using Blueboard.Core.Realtime.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Blueboard.Core.Realtime.Hubs;

[Authorize]
public class NotificationsHub : Hub<INotificationsClient>
{
}