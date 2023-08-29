using System.Text.Json;
using Blueboard.Infrastructure.Persistence.Entities;
using FluentEmail.Core;
using FluentEmail.Core.Models;
using Helpers.Email.Services;
using Helpers.Email.Views.Emails.OwnedItemUsedNotification;
using Quartz;

namespace Blueboard.Features.Shop.Jobs;

public class SendOwnedItemUsedNotificationJob : IJob
{
    private readonly IFluentEmail _fluentEmail;
    private readonly RazorViewToStringRenderer _razorViewToStringRenderer;

    public SendOwnedItemUsedNotificationJob(IFluentEmail fluentEmail,
        RazorViewToStringRenderer razorViewToStringRenderer)
    {
        _fluentEmail = fluentEmail;
        _razorViewToStringRenderer = razorViewToStringRenderer;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var user = JsonSerializer.Deserialize<User>((context.MergedJobDataMap.Get("userJson") as string)!);
        var product = JsonSerializer.Deserialize<Product>((context.MergedJobDataMap.Get("productJson") as string)!);
        var inputValues =
            JsonSerializer.Deserialize<Dictionary<string, string>>(
                (context.MergedJobDataMap.Get("inputValuesJson") as string)!);
        var qrCodeEmail = context.MergedJobDataMap.Get("qrCodeEmail") as string; // Can be null

        var addresses = product!.NotifiedEmails.Select(e => new Address(e)).ToList();
        if (qrCodeEmail != null) addresses.Add(new Address(qrCodeEmail));

        if (addresses.Count == 0) return;

        var email = _fluentEmail.BCC(addresses.Distinct()).Subject($"Termék felhasználva: {product.Name}").Body(
            await _razorViewToStringRenderer.RenderViewToStringAsync(
                "/Views/Emails/OwnedItemUsedNotification/OwnedItemUsedNotification.cshtml",
                new OwnedItemUsedNotificationViewModel
                {
                    ProductName = product.Name,
                    ProductDescription = product.Description,
                    UserDisplayName = user!.RealName ?? "<" + user.Name + ">",
                    UserClass = user.Class,
                    InputValues = inputValues!
                }), true);

        await email.SendAsync();
    }
}