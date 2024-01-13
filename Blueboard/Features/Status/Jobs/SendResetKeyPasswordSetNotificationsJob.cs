using Blueboard.Infrastructure.Persistence;
using FluentEmail.Core;
using FluentEmail.Core.Models;
using Helpers.Email.Services;
using Helpers.Email.Views.Emails.ResetKeyPasswordSetNotification;
using Microsoft.EntityFrameworkCore;
using Quartz;
using Shimmer.Core;

namespace Blueboard.Features.Status.Jobs;

public class SendResetKeyPasswordSetNotificationsJob(
    ApplicationDbContext dbContext,
    IFluentEmail fluentEmail,
    RazorViewToStringRenderer razorViewToStringRenderer)
    : ShimmerJob
{
    protected override async Task Process(IJobExecutionContext context)
    {
        var notifiers = await dbContext.ResetKeyPasswordSetNotifiers.ToListAsync();

        if (notifiers.Count == 0) return;

        var addresses = notifiers.Select(n => new Address(n.Email)).ToList();

        var email = fluentEmail.BCC(addresses).Subject("A visszaállítási jelszó be lett állítva")
            .Body(
                await razorViewToStringRenderer.RenderViewToStringAsync(
                    "/Views/Emails/ResetKeyPasswordSetNotification/ResetKeyPasswordSetNotification.cshtml",
                    new ResetKeyPasswordSetNotificationViewModel()), true);

        await email.SendAsync();

        dbContext.ResetKeyPasswordSetNotifiers.RemoveRange(notifiers);
        await dbContext.SaveChangesAsync();
    }
}