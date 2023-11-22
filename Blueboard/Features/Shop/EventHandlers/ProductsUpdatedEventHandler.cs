using Blueboard.Core.Auth.Permissions;
using Blueboard.Core.Auth.Utils;
using Blueboard.Core.Realtime.Hubs;
using Blueboard.Core.Realtime.Interfaces;
using Blueboard.Features.Shop.Events;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Blueboard.Features.Shop.EventHandlers;

public class ProductsUpdatedEventHandler(IHubContext<NotificationsHub, INotificationsClient> notificationsHub)
    : INotificationHandler<ProductsUpdatedEvent>
{
    public async Task Handle(ProductsUpdatedEvent notification, CancellationToken cancellationToken)
    {
        if (PermissionUtils.PermissionTypesToNames == null)
            throw new InvalidOperationException("Permissions are not loaded yet");

        await notificationsHub.Clients
            .Groups(PermissionUtils.PermissionTypesToNames[typeof(ShopPermissions.IndexStoreProducts)],
                PermissionUtils.PermissionTypesToNames[typeof(ShopPermissions.IndexProducts)]).RefreshProducts();
    }
}