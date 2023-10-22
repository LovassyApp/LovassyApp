using System.Text.Json;
using Blueboard.Infrastructure.Persistence;
using Blueboard.Infrastructure.Persistence.Entities;
using FluentEmail.Core;
using FluentEmail.Core.Models;
using Helpers.Email.Services;
using Helpers.Email.Views.Emails.LoloRequestCreatedNotification;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace Blueboard.Features.Shop.Jobs;

public class SendLoloRequestCreatedNotificationJob : IJob
{
    private readonly ApplicationDbContext _context;
    private readonly IFluentEmail _fluentEmail;
    private readonly RazorViewToStringRenderer _razorViewToStringRenderer;

    public SendLoloRequestCreatedNotificationJob(IFluentEmail fluentEmail,
        RazorViewToStringRenderer razorViewToStringRenderer, ApplicationDbContext context)
    {
        _fluentEmail = fluentEmail;
        _razorViewToStringRenderer = razorViewToStringRenderer;
        _context = context;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var loloRequest =
            JsonSerializer.Deserialize<LoloRequest>((context.MergedJobDataMap.Get("loloRequest") as string)!);
        var loloRequestsUrl = context.MergedJobDataMap.Get("loloRequestsUrl") as string;


        var addresses = (await _context.LoloRequestCreatedNotifiers.AsNoTracking().ToListAsync())
            .Select(e => new Address(e.Email)).ToList();

        if (addresses.Count == 0) return;

        var email = _fluentEmail.BCC(addresses).Subject($"Kérvény létrehozva: {loloRequest!.Title}").Body(
            await _razorViewToStringRenderer.RenderViewToStringAsync(
                "/Views/Emails/LoloRequestCreatedNotification/LoloRequestCreatedNotification.cshtml",
                new LoloRequestCreatedNotificationViewModel
                {
                    Title = loloRequest.Title,
                    Body = loloRequest.Body,
                    LoloRequestsUrl = loloRequestsUrl!
                }), true);

        await email.SendAsync();
    }
}