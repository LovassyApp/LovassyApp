using System.Globalization;
using Blueboard.Core.Auth.Events;
using Blueboard.Core.Auth.Jobs;
using MediatR;
using Quartz;

namespace Blueboard.Core.Auth.EventHandlers;

public class AccessTokenUsedEventHandler(ISchedulerFactory schedulerFactory) : INotificationHandler<AccessTokenUsedEvent>
{
    public async Task Handle(AccessTokenUsedEvent notification, CancellationToken cancellationToken)
    {
        var scheduler = await schedulerFactory.GetScheduler(cancellationToken);

        var updateTokenLastUsedAtJob = JobBuilder.Create<UpdateTokenLastUsedAtJob>()
            .UsingJobData("id", notification.AccessToken.Id)
            .UsingJobData("lastUsedAt",
                DateTime.Now.ToString(CultureInfo.InvariantCulture)) // It gets converted to utc in the job
            .Build();

        var updateTokenLastUsedAtTrigger = TriggerBuilder.Create()
            .StartNow()
            .Build();

        await scheduler.ScheduleJob(updateTokenLastUsedAtJob, updateTokenLastUsedAtTrigger, cancellationToken);
    }
}