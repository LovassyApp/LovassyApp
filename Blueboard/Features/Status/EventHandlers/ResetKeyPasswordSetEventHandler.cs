using Blueboard.Features.Import.Events;
using Blueboard.Features.Status.Jobs;
using MediatR;
using Quartz;

namespace Blueboard.Features.Status.EventHandlers;

public class ResetKeyPasswordSetEventHandler : INotificationHandler<ResetKeyPasswordSetEvent>
{
    private readonly ISchedulerFactory _schedulerFactory;

    public ResetKeyPasswordSetEventHandler(ISchedulerFactory schedulerFactory)
    {
        _schedulerFactory = schedulerFactory;
    }

    public async Task Handle(ResetKeyPasswordSetEvent notification, CancellationToken cancellationToken)
    {
        var scheduler = await _schedulerFactory.GetScheduler(cancellationToken);

        var sendResetKeyPasswordSetNotificationsJob = JobBuilder.Create<SendResetKeyPasswordSetNotificationsJob>()
            .Build();

        var sendResetKeyPasswordSetNotificationsTrigger = TriggerBuilder.Create().StartNow().Build();

        await scheduler.ScheduleJob(sendResetKeyPasswordSetNotificationsJob,
            sendResetKeyPasswordSetNotificationsTrigger, cancellationToken);
    }
}