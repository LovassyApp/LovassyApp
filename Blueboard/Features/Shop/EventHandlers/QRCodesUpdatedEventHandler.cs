using Blueboard.Core.Auth.Permissions;
using Blueboard.Core.Auth.Utils;
using Blueboard.Core.Realtime.Hubs;
using Blueboard.Core.Realtime.Interfaces;
using Blueboard.Features.Shop.Events;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Blueboard.Features.Shop.EventHandlers;

public class QRCodesUpdatedEventHandler : INotificationHandler<QRCodesUpdatedEvent>
{
    private readonly IHubContext<NotificationsHub, INotificationsClient> _notificationsHub;

    public QRCodesUpdatedEventHandler(IHubContext<NotificationsHub, INotificationsClient> notificationsHub)
    {
        _notificationsHub = notificationsHub;
    }

    public async Task Handle(QRCodesUpdatedEvent notification, CancellationToken cancellationToken)
    {
        if (PermissionUtils.PermissionTypesToNames == null)
            throw new InvalidOperationException("Permissions are not loaded yet");

        await _notificationsHub.Clients
            .Group(PermissionUtils.PermissionTypesToNames[typeof(ShopPermissions.IndexQRCodes)]).RefreshQRCodes();
    }
}