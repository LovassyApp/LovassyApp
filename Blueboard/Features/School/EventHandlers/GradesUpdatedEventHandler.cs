using Blueboard.Core.Realtime.Hubs;
using Blueboard.Core.Realtime.Interfaces;
using Blueboard.Features.School.Events;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Blueboard.Features.School.EventHandlers;

public class GradesUpdatedEventHandler(IHubContext<NotificationsHub, INotificationsClient> notificationsHub)
    : INotificationHandler<GradesUpdatedEvent>
{
    public async Task Handle(GradesUpdatedEvent notification, CancellationToken cancellationToken)
    {
        await notificationsHub.Clients.User(notification.UserId.ToString()).RefreshGrades();
    }
}