using Blueboard.Core.Realtime.Hubs;
using Blueboard.Core.Realtime.Interfaces;
using Blueboard.Features.Auth.Events;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Blueboard.Features.Auth.EventHandlers;

public class LolosUpdatedEventHandler : INotificationHandler<LolosUpdatedEvent>
{
    private readonly IHubContext<NotificationsHub, INotificationsClient> _notificationsHub;

    public LolosUpdatedEventHandler(IHubContext<NotificationsHub, INotificationsClient> notificationsHub)
    {
        _notificationsHub = notificationsHub;
    }

    public async Task Handle(LolosUpdatedEvent notification, CancellationToken cancellationToken)
    {
        await _notificationsHub.Clients.User(notification.UserId.ToString()).RefreshLolos();
    }
}