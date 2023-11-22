using System.Text.Json;
using Blueboard.Features.Auth.Jobs;
using Blueboard.Features.Users.Events;
using MediatR;
using Quartz;

namespace Blueboard.Features.Auth.EventHandlers;

public class UserCreatedEventHandler(ISchedulerFactory schedulerFactory) : INotificationHandler<UserCreatedEvent>
{
    public async Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
    {
        var scheduler = await schedulerFactory.GetScheduler(cancellationToken);

        var sendVerifyEmailJob = JobBuilder.Create<SendVerifyEmailJob>()
            .UsingJobData("userJson", JsonSerializer.Serialize(notification.User))
            .UsingJobData("verifyUrl", notification.VerifyUrl)
            .UsingJobData("verifyTokenQueryKey", notification.VerifyTokenQueryKey)
            .Build();

        var sendVerifyEmailTrigger = TriggerBuilder.Create()
            .StartNow()
            .Build();

        await scheduler.ScheduleJob(sendVerifyEmailJob, sendVerifyEmailTrigger, cancellationToken);
    }
}