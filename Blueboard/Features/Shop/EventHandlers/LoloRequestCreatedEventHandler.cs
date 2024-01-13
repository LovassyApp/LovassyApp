using Blueboard.Features.Shop.Events;
using Blueboard.Features.Shop.Jobs;
using MediatR;
using Shimmer.Services;

namespace Blueboard.Features.Shop.EventHandlers;

public class LoloRequestCreatedEventHandler(IShimmerJobFactory jobFactory)
    : INotificationHandler<LoloRequestCreatedEvent>
{
    public async Task Handle(LoloRequestCreatedEvent notification, CancellationToken cancellationToken)
    {
        var sendLoloRequestCreatedNotificationJob =
            await jobFactory.CreateAsync<SendLoloRequestCreatedNotificationJob,
                SendLoloRequestCreatedNotificationJob.Data>(cancellationToken);

        sendLoloRequestCreatedNotificationJob.Data(new SendLoloRequestCreatedNotificationJob.Data
        {
            LoloRequest = notification.LoloRequest,
            LoloRequestsUrl = notification.LoloRequestsUrl
        });

        await sendLoloRequestCreatedNotificationJob.FireAsync(cancellationToken);
    }
}