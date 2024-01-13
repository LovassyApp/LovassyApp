using Blueboard.Infrastructure.Persistence.Entities;
using FluentEmail.Core;
using FluentEmail.Core.Models;
using Helpers.Email.Services;
using Helpers.Email.Views.Emails.OwnedItemUsedNotification;
using Quartz;
using Shimmer.Core;

namespace Blueboard.Features.Shop.Jobs;

public class SendOwnedItemUsedNotificationJob(
    IFluentEmail fluentEmail,
    RazorViewToStringRenderer razorViewToStringRenderer)
    : ShimmerJob<SendOwnedItemUsedNotificationJob.Data>
{
    protected override async Task Process(Data data, IJobExecutionContext context)
    {
        var addresses = data.Product.NotifiedEmails.Select(e => new Address(e)).ToList();
        if (data.QrCodeEmail != null) addresses.Add(new Address(data.QrCodeEmail));

        if (addresses.Count == 0) return;

        var email = fluentEmail.BCC(addresses.Distinct()).Subject($"Termék felhasználva: {data.Product.Name}").Body(
            await razorViewToStringRenderer.RenderViewToStringAsync(
                "/Views/Emails/OwnedItemUsedNotification/OwnedItemUsedNotification.cshtml",
                new OwnedItemUsedNotificationViewModel
                {
                    ProductName = data.Product.Name,
                    ProductDescription = data.Product.Description,
                    UserDisplayName = data.User.RealName ?? "<" + data.User.Name + ">",
                    UserClass = data.User.Class,
                    InputValues = data.InputValues
                }), true);

        await email.SendAsync();
    }

    public class Data
    {
        public User User { get; set; }
        public Product Product { get; set; }
        public Dictionary<string, string> InputValues { get; set; }
        public string? QrCodeEmail { get; set; }
    }
}