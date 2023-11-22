using System.Text.Json;
using Blueboard.Features.Shop.Events;
using Blueboard.Features.Shop.Jobs;
using MediatR;
using Quartz;

namespace Blueboard.Features.Shop.EventHandlers;

public class OwnedItemUsedEventHandler(ISchedulerFactory schedulerFactory) : INotificationHandler<OwnedItemUsedEvent>
{
    public async Task Handle(OwnedItemUsedEvent notification, CancellationToken cancellationToken)
    {
        var scheduler = await schedulerFactory.GetScheduler(cancellationToken);

        var sendOwnedItemUsedNotificationJob = JobBuilder.Create<SendOwnedItemUsedNotificationJob>()
            .WithIdentity($"SendOwnedItemUseNotificationJob-{notification.User.Id}-{notification.Product.Id}")
            .UsingJobData("userJson", JsonSerializer.Serialize(notification.User))
            .UsingJobData("productJson", JsonSerializer.Serialize(notification.Product))
            .UsingJobData("inputValuesJson", JsonSerializer.Serialize(notification.InputValues))
            .UsingJobData("qrCodeEmail", notification.QRCodeEmail)
            .Build();

        var sendOwnedItemUsedNotificationTrigger = TriggerBuilder.Create().StartNow().Build();

        await scheduler.ScheduleJob(sendOwnedItemUsedNotificationJob, sendOwnedItemUsedNotificationTrigger,
            cancellationToken);
    }
}