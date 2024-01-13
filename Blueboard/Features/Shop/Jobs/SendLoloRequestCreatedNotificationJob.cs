using Blueboard.Infrastructure.Persistence;
using Blueboard.Infrastructure.Persistence.Entities;
using FluentEmail.Core;
using FluentEmail.Core.Models;
using Helpers.Email.Services;
using Helpers.Email.Views.Emails.LoloRequestCreatedNotification;
using Microsoft.EntityFrameworkCore;
using Quartz;
using Shimmer.Core;

namespace Blueboard.Features.Shop.Jobs;

public class SendLoloRequestCreatedNotificationJob(
    IFluentEmail fluentEmail,
    RazorViewToStringRenderer razorViewToStringRenderer,
    ApplicationDbContext dbContext)
    : ShimmerJob<SendLoloRequestCreatedNotificationJob.Data>
{
    protected override async Task Process(Data data, IJobExecutionContext context)
    {
        var addresses = (await dbContext.LoloRequestCreatedNotifiers.AsNoTracking().ToListAsync())
            .Select(e => new Address(e.Email)).ToList();

        if (addresses.Count == 0) return;

        var email = fluentEmail.BCC(addresses).Subject($"Kérvény létrehozva: {data.LoloRequest.Title}").Body(
            await razorViewToStringRenderer.RenderViewToStringAsync(
                "/Views/Emails/LoloRequestCreatedNotification/LoloRequestCreatedNotification.cshtml",
                new LoloRequestCreatedNotificationViewModel
                {
                    Title = data.LoloRequest.Title,
                    Body = data.LoloRequest.Body,
                    LoloRequestsUrl = data.LoloRequestsUrl
                }), true);

        await email.SendAsync();
    }

    public class Data
    {
        public LoloRequest LoloRequest { get; set; }
        public string LoloRequestsUrl { get; set; }
    }
}