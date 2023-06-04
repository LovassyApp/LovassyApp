using System.Globalization;
using Blueboard.Core.Auth.Events;
using Blueboard.Core.Auth.Jobs;
using MediatR;
using Quartz;

namespace Blueboard.Core.Auth.EventHandlers;

public class AccessTokenUsedEventHandler : INotificationHandler<AccessTokenUsedEvent>
{
    private readonly ISchedulerFactory _schedulerFactory;

    public AccessTokenUsedEventHandler(ISchedulerFactory schedulerFactory)
    {
        _schedulerFactory = schedulerFactory;
    }

    public async Task Handle(AccessTokenUsedEvent notification, CancellationToken cancellationToken)
    {
        var scheduler = await _schedulerFactory.GetScheduler();

        var updateTokenLastUsedAtJob = JobBuilder.Create<UpdateTokenLastUsedAtJob>()
            .WithIdentity("updateTokenLastUsedAtJob", "onTokenUsedJobs")
            .UsingJobData("id", notification.AccessToken.Id)
            .UsingJobData("lastUsedAt",
                DateTime.Now.ToString(CultureInfo.InvariantCulture)) // It gets converted to utc in the job
            .Build();

        var updateTokenLastUsedAtTrigger = TriggerBuilder.Create()
            .WithIdentity("updateTokenLastUsedAtTrigger", "onTokenUsedJobs")
            .StartNow()
            .Build();

        await scheduler.ScheduleJob(updateTokenLastUsedAtJob, updateTokenLastUsedAtTrigger, cancellationToken);
    }
}