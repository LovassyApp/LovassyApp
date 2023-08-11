using Blueboard.Features.Realtime.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Blueboard.Features.Realtime;

[Authorize]
public class NotificationsHub : Hub<INotificationsClient>
{
}