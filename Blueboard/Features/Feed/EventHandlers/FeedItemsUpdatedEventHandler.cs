using Blueboard.Core.Auth.Permissions;
using Blueboard.Core.Auth.Utils;
using Blueboard.Core.Realtime.Hubs;
using Blueboard.Core.Realtime.Interfaces;
using Blueboard.Features.Feed.Events;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Blueboard.Features.Feed.EventHandlers;

public class FeedItemsUpdatedEventHandler : INotificationHandler<FeedItemsUpdatedEvent>
{
    private readonly IHubContext<NotificationsHub, INotificationsClient> _notificationsHub;

    public FeedItemsUpdatedEventHandler(IHubContext<NotificationsHub, INotificationsClient> notificationsHub)
    {
        _notificationsHub = notificationsHub;
    }

    public async Task Handle(FeedItemsUpdatedEvent notification, CancellationToken cancellationToken)
    {
        await _notificationsHub.Clients
            .Group(PermissionUtils.PermissionTypesToNames[typeof(FeedPermissions.IndexFeedItems)])
            .RefreshFeedItems();
    }
}