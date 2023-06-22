using Blueboard.Infrastructure.Persistence;
using FluentEmail.Core;
using FluentEmail.Core.Models;
using Helpers.Email.Services;
using Helpers.Email.Views.Emails.ResetKeyPasswordSetNotification;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace Blueboard.Features.Status.Jobs;

public class SendResetKeyPasswordSetNotificationsJob : IJob
{
    private readonly ApplicationDbContext _context;
    private readonly IFluentEmailFactory _emailFactory;
    private readonly RazorViewToStringRenderer _razorViewToStringRenderer;

    public SendResetKeyPasswordSetNotificationsJob(ApplicationDbContext context, IFluentEmailFactory emailFactory,
        RazorViewToStringRenderer razorViewToStringRenderer)
    {
        _context = context;
        _emailFactory = emailFactory;
        _razorViewToStringRenderer = razorViewToStringRenderer;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var notifiers = await _context.ResetKeyPasswordSetNotifiers.ToListAsync();

        var addresses = notifiers.Select(n => new Address(n.Email)).ToList();

        var email = _emailFactory.Create().BCC(addresses).Subject("A visszaállítási jelszó be lett állítva")
            .Body(
                await _razorViewToStringRenderer.RenderViewToStringAsync(
                    "/Views/Emails/ResetKeyPasswordSetNotification/ResetKeyPasswordSetNotification.cshtml",
                    new ResetKeyPasswordSetNotificationViewModel()), true);

        await email.SendAsync();

        _context.ResetKeyPasswordSetNotifiers.RemoveRange(notifiers);
        await _context.SaveChangesAsync();
    }
}