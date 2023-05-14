using System.Text.Json;
using MediatR;
using Quartz;
using Quartz.Impl.Matchers;
using Quartz.Listener;
using WebApi.Features.Auth.Events;
using WebApi.Features.Auth.Jobs;

namespace WebApi.Features.Auth.EventHandlers;

public class SessionCreatedEventHandler : INotificationHandler<SessionCreatedEvent>
{
    private readonly ISchedulerFactory _schedulerFactory;

    public SessionCreatedEventHandler(ISchedulerFactory schedulerFactory)
    {
        _schedulerFactory = schedulerFactory;
    }

    public async Task Handle(SessionCreatedEvent notification, CancellationToken cancellationToken)
    {
        var scheduler = await _schedulerFactory.GetScheduler(cancellationToken);

        var updateGradesJob = JobBuilder.Create<UpdateGradesJob>()
            .WithIdentity("updateGrades", "sessionCreatedJobs")
            .UsingJobData("userJson", JsonSerializer.Serialize(notification.User))
            .UsingJobData("masterKey", notification.MasterKey)
            .Build();

        var updateGradesTrigger = TriggerBuilder.Create().WithIdentity("updateGradesTrigger", "sessionCreatedJobs")
            .StartNow()
            .Build();

        var updateLolosJob = JobBuilder.Create<UpdateLolosJob>()
            .WithIdentity("updateLolos", "sessionCreatedJobs")
            .UsingJobData("userJson", JsonSerializer.Serialize(notification.User))
            .UsingJobData("masterKey", notification.MasterKey)
            .Build();

        var chainingListener = new JobChainingJobListener("sessionCreatedJobsPipeline");
        chainingListener.AddJobChainLink(updateGradesJob.Key, updateLolosJob.Key);

        scheduler.ListenerManager.AddJobListener(chainingListener,
            GroupMatcher<JobKey>.GroupEquals("sessionCreatedJobs"));

        await scheduler.ScheduleJob(updateGradesJob, updateGradesTrigger, cancellationToken);
        await scheduler.AddJob(updateLolosJob, false, true, cancellationToken);
    }
}