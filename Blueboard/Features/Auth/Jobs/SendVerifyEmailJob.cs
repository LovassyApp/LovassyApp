using Blueboard.Features.Auth.Services;
using Blueboard.Infrastructure.Persistence.Entities;
using FluentEmail.Core;
using Helpers.Email.Services;
using Helpers.Email.Views.Emails.VerifyEmail;
using Quartz;
using Shimmer.Core;

namespace Blueboard.Features.Auth.Jobs;

public class SendVerifyEmailJob(
    IFluentEmail fluentEmail,
    VerifyEmailService verifyEmailService,
    RazorViewToStringRenderer razorViewToStringRenderer)
    : ShimmerJob<SendVerifyEmailJob.Data>
{
    protected override async Task Process(Data data, IJobExecutionContext context)
    {
        var verifyToken = verifyEmailService.GenerateVerifyToken(data.User.Id);

        var email = fluentEmail.To(data.User.Email).Subject("Email megerősítése").Body(
            await razorViewToStringRenderer.RenderViewToStringAsync("/Views/Emails/VerifyEmail/VerifyEmail.cshtml",
                new VerifyEmailViewModel
                {
                    VerifyEmailUrl = $"{data.VerifyUrl}?{data.VerifyTokenQueryKey}={verifyToken}"
                }), true);

        await email.SendAsync();
    }

    public class Data
    {
        public User User { get; set; }
        public string VerifyUrl { get; set; }
        public string VerifyTokenQueryKey { get; set; }
    }
}