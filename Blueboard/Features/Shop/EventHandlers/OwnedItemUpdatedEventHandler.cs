using Blueboard.Core.Auth.Permissions;
using Blueboard.Core.Auth.Utils;
using Blueboard.Core.Realtime.Hubs;
using Blueboard.Core.Realtime.Interfaces;
using Blueboard.Features.Shop.Events;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Blueboard.Features.Shop.EventHandlers;

public class OwnedItemUpdatedEventHandler(IHubContext<NotificationsHub, INotificationsClient> notificationsHub)
    : INotificationHandler<OwnedItemUpdatedEvent>
{
    public async Task Handle(OwnedItemUpdatedEvent notification, CancellationToken cancellationToken)
    {
        if (PermissionUtils.PermissionTypesToNames == null)
            throw new InvalidOperationException("Permissions are not loaded yet");

        await notificationsHub.Clients.User(notification.UserId.ToString()).RefreshOwnOwnedItems();
        await notificationsHub.Clients
            .Group(PermissionUtils.PermissionTypesToNames[typeof(ShopPermissions.IndexOwnedItems)])
            .RefreshOwnedItems();
    }
}