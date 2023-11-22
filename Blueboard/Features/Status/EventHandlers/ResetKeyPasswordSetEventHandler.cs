using Blueboard.Features.Import.Events;
using Blueboard.Features.Status.Jobs;
using MediatR;
using Quartz;

namespace Blueboard.Features.Status.EventHandlers;

public class ResetKeyPasswordSetEventHandler(ISchedulerFactory schedulerFactory) : INotificationHandler<ResetKeyPasswordSetEvent>
{
    public async Task Handle(ResetKeyPasswordSetEvent notification, CancellationToken cancellationToken)
    {
        var scheduler = await schedulerFactory.GetScheduler(cancellationToken);

        var sendResetKeyPasswordSetNotificationsJob = JobBuilder.Create<SendResetKeyPasswordSetNotificationsJob>()
            .Build();

        var sendResetKeyPasswordSetNotificationsTrigger = TriggerBuilder.Create().StartNow().Build();

        await scheduler.ScheduleJob(sendResetKeyPasswordSetNotificationsJob,
            sendResetKeyPasswordSetNotificationsTrigger, cancellationToken);
    }
}