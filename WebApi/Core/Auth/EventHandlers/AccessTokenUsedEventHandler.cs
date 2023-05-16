using System.Globalization;
using MediatR;
using Quartz;
using WebApi.Core.Auth.Events;
using WebApi.Core.Auth.Jobs;

namespace WebApi.Core.Auth.EventHandlers;

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