using Blueboard.Core.Auth.Permissions;
using Blueboard.Core.Auth.Utils;
using Blueboard.Core.Realtime.Hubs;
using Blueboard.Core.Realtime.Interfaces;
using Blueboard.Features.ImageVotings.Events;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Blueboard.Features.ImageVotings.EventHandlers;

public class ImageVotingsUpdatedEventHandler(IHubContext<NotificationsHub, INotificationsClient> notificationsHub)
    : INotificationHandler<ImageVotingsUpdatedEvent>
{
    public async Task Handle(ImageVotingsUpdatedEvent notification, CancellationToken cancellationToken)
    {
        if (PermissionUtils.PermissionTypesToNames == null)
            throw new InvalidOperationException("Permissions are not loaded yet");

        await notificationsHub.Clients
            .Groups(PermissionUtils.PermissionTypesToNames[typeof(ImageVotingsPermissions.IndexImageVotings)],
                PermissionUtils.PermissionTypesToNames[typeof(ImageVotingsPermissions.IndexActiveImageVotings)])
            .RefreshImageVotings();
    }
}