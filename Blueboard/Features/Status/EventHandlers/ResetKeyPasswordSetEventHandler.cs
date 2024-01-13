using Blueboard.Features.Import.Events;
using Blueboard.Features.Status.Jobs;
using MediatR;
using Shimmer.Services;

namespace Blueboard.Features.Status.EventHandlers;

public class ResetKeyPasswordSetEventHandler(IShimmerJobFactory jobFactory)
    : INotificationHandler<ResetKeyPasswordSetEvent>
{
    public async Task Handle(ResetKeyPasswordSetEvent notification, CancellationToken cancellationToken)
    {
        var sendResetKeyPasswordSetNotificationsJob =
            await jobFactory.CreateAsync<SendResetKeyPasswordSetNotificationsJob>(cancellationToken);

        await sendResetKeyPasswordSetNotificationsJob.FireAsync(cancellationToken);
    }
}