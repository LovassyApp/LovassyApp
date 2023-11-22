using Blueboard.Core.Auth.Permissions;
using Blueboard.Core.Auth.Utils;
using Blueboard.Core.Realtime.Hubs;
using Blueboard.Core.Realtime.Interfaces;
using Blueboard.Features.ImageVotings.Events;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Blueboard.Features.ImageVotings.EventHandlers;

public class ImageVotingChoicesUpdatedEventHandler(IHubContext<NotificationsHub, INotificationsClient> notificationsHub)
    : INotificationHandler<ImageVotingChoicesUpdatedEvent>
{
    public async Task Handle(ImageVotingChoicesUpdatedEvent notification, CancellationToken cancellationToken)
    {
        if (PermissionUtils.PermissionTypesToNames == null)
            throw new InvalidOperationException("Permissions are not loaded yet");

        await notificationsHub.Clients
            .Groups(PermissionUtils.PermissionTypesToNames[typeof(ImageVotingsPermissions.IndexImageVotingChoices)],
                PermissionUtils.PermissionTypesToNames[typeof(ImageVotingsPermissions.IndexActiveImageVotingChoices)])
            .RefreshImageVotingChoices();

        await notificationsHub.Clients.Groups(
                PermissionUtils.PermissionTypesToNames[typeof(ImageVotingsPermissions.IndexImageVotingEntries)],
                PermissionUtils.PermissionTypesToNames[typeof(ImageVotingsPermissions.IndexActiveImageVotingEntries)])
            .RefreshImageVotingEntries();
    }
}