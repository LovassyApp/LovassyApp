using System.Text.Json;
using MediatR;
using Quartz;
using WebApi.Features.Auth.Jobs;
using WebApi.Features.Users.Commands;
using WebApi.Infrastructure.Persistence.Entities;

namespace WebApi.Features.Auth.EventHandlers;

public class UserCreatedEventHandler : INotificationHandler<UserCreatedEvent>
{
    private readonly ISchedulerFactory _schedulerFactory;

    public UserCreatedEventHandler(ISchedulerFactory schedulerFactory)
    {
        _schedulerFactory = schedulerFactory;
    }

    public async Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
    {
        await ScheduleUserCreatedJobsAsync(notification.User, cancellationToken);
    }

    private async Task ScheduleUserCreatedJobsAsync(User user,
        CancellationToken cancellationToken)
    {
        var scheduler = await _schedulerFactory.GetScheduler(cancellationToken);

        var sendVerifyEmailJob = JobBuilder.Create<SendVerifyEmailJob>()
            .WithIdentity("sendVerifyEmail", "userCreatedJobs")
            .UsingJobData("userJson", JsonSerializer.Serialize(user))
            .Build();

        var sendVerifyEmailTrigger = TriggerBuilder.Create()
            .WithIdentity("sendVerifyEmailTrigger", "userCreatedJobs")
            .StartNow()
            .Build();

        await scheduler.ScheduleJob(sendVerifyEmailJob, sendVerifyEmailTrigger, cancellationToken);
    }
}