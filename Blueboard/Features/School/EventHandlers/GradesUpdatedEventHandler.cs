using Blueboard.Core.Realtime.Hubs;
using Blueboard.Core.Realtime.Interfaces;
using Blueboard.Features.School.Events;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Blueboard.Features.School.EventHandlers;

public class GradesUpdatedEventHandler : INotificationHandler<GradesUpdatedEvent>
{
    private readonly IHubContext<NotificationsHub, INotificationsClient> _notificationsHub;

    public GradesUpdatedEventHandler(IHubContext<NotificationsHub, INotificationsClient> notificationsHub)
    {
        _notificationsHub = notificationsHub;
    }

    public async Task Handle(GradesUpdatedEvent notification, CancellationToken cancellationToken)
    {
        await _notificationsHub.Clients.User(notification.UserId.ToString()).RefreshGrades();
    }
}