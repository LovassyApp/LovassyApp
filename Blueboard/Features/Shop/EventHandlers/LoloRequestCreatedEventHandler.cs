using System.Text.Json;
using Blueboard.Features.Shop.Events;
using Blueboard.Features.Shop.Jobs;
using MediatR;
using Quartz;

namespace Blueboard.Features.Shop.EventHandlers;

public class LoloRequestCreatedEventHandler(ISchedulerFactory schedulerFactory) : INotificationHandler<LoloRequestCreatedEvent>
{
    public async Task Handle(LoloRequestCreatedEvent notification, CancellationToken cancellationToken)
    {
        var scheduler = await schedulerFactory.GetScheduler(cancellationToken);

        var sendLoloRequestCreatedNotificationJob = JobBuilder.Create<SendLoloRequestCreatedNotificationJob>()
            .WithIdentity($"SendLoloRequestCreatedNotification-{notification.LoloRequest.Id}")
            .UsingJobData("loloRequest", JsonSerializer.Serialize(notification.LoloRequest))
            .UsingJobData("loloRequestsUrl", notification.LoloRequestsUrl)
            .Build();

        var sendLoloRequestCreatedNotificationTrigger = TriggerBuilder.Create().StartNow().Build();

        await scheduler.ScheduleJob(sendLoloRequestCreatedNotificationJob, sendLoloRequestCreatedNotificationTrigger,
            cancellationToken);
    }
}