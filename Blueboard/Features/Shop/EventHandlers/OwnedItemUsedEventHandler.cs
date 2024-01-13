using Blueboard.Features.Shop.Events;
using Blueboard.Features.Shop.Jobs;
using MediatR;
using Shimmer.Services;

namespace Blueboard.Features.Shop.EventHandlers;

public class OwnedItemUsedEventHandler(IShimmerJobFactory jobFactory) : INotificationHandler<OwnedItemUsedEvent>
{
    public async Task Handle(OwnedItemUsedEvent notification, CancellationToken cancellationToken)
    {
        var sendOwnedItemUsedNotificationJob =
            await jobFactory.CreateAsync<SendOwnedItemUsedNotificationJob,
                SendOwnedItemUsedNotificationJob.Data>(cancellationToken);

        sendOwnedItemUsedNotificationJob.Data(new SendOwnedItemUsedNotificationJob.Data
        {
            User = notification.User,
            Product = notification.Product,
            InputValues = notification.InputValues,
            QrCodeEmail = notification.QRCodeEmail
        });

        await sendOwnedItemUsedNotificationJob.FireAsync(cancellationToken);
    }
}