using System.Text.Json;
using Blueboard.Features.Auth.Events;
using Blueboard.Features.Auth.Jobs;
using MediatR;
using Quartz;
using Quartz.Impl.Matchers;
using Quartz.Listener;

namespace Blueboard.Features.Auth.EventHandlers;

public class SessionCreatedEventHandler(ISchedulerFactory schedulerFactory) : INotificationHandler<SessionCreatedEvent>
{
    public async Task Handle(SessionCreatedEvent notification, CancellationToken cancellationToken)
    {
        var scheduler = await schedulerFactory.GetScheduler(cancellationToken);

        // We need this because we want to be able to concurrently run the same job
        var idSalt = Guid.NewGuid().ToString();

        var updateGradesJob = JobBuilder.Create<UpdateGradesJob>()
            .WithIdentity("updateGrades" + idSalt, "sessionCreatedJobs")
            .UsingJobData("userJson", JsonSerializer.Serialize(notification.User))
            .UsingJobData("masterKey", notification.MasterKey)
            .Build();

        var updateGradesTrigger = TriggerBuilder.Create()
            .WithIdentity("updateGradesTrigger" + idSalt, "sessionCreatedJobs")
            .StartNow()
            .Build();

        var updateLolosJob = JobBuilder.Create<UpdateLolosJob>()
            .WithIdentity("updateLolos" + idSalt, "sessionCreatedJobs")
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