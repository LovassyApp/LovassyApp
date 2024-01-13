using Blueboard.Features.Auth.Services;
using Blueboard.Infrastructure.Persistence.Entities;
using FluentEmail.Core;
using Helpers.Email.Services;
using Helpers.Email.Views.Emails.PasswordReset;
using Quartz;
using Shimmer.Core;

namespace Blueboard.Features.Auth.Jobs;

public class SendPasswordResetJob(
    IFluentEmail fluentEmail,
    PasswordResetService passwordResetService,
    RazorViewToStringRenderer razorViewToStringRenderer)
    : ShimmerJob<SendPasswordResetJob.Data>
{
    protected override async Task Process(Data data, IJobExecutionContext context)
    {
        var passwordResetToken = passwordResetService.GeneratePasswordResetToken(data.User!.Id);

        var email = fluentEmail.To(data.User.Email).Subject("Jelszó visszaállítása").Body(
            await razorViewToStringRenderer.RenderViewToStringAsync("/Views/Emails/PasswordReset/PasswordReset.cshtml",
                new PasswordResetViewModel
                {
                    PasswordResetUrl = $"{data.PasswordResetUrl}?{data.PasswordResetTokenQueryKey}={passwordResetToken}"
                }), true);

        await email.SendAsync();
    }

    public class Data
    {
        public User User { get; set; }
        public string PasswordResetUrl { get; set; }
        public string PasswordResetTokenQueryKey { get; set; }
    }
}