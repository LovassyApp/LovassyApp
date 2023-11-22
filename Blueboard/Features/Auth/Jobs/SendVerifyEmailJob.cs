using System.Text.Json;
using Blueboard.Features.Auth.Services;
using Blueboard.Infrastructure.Persistence.Entities;
using FluentEmail.Core;
using Helpers.Email.Services;
using Helpers.Email.Views.Emails.VerifyEmail;
using Quartz;

namespace Blueboard.Features.Auth.Jobs;

public class SendVerifyEmailJob(IFluentEmail fluentEmail, VerifyEmailService verifyEmailService,
        RazorViewToStringRenderer razorViewToStringRenderer)
    : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        var user = JsonSerializer.Deserialize<User>((context.MergedJobDataMap.Get("userJson") as string)!);
        var verifyUrl = context.MergedJobDataMap.Get("verifyUrl") as string;
        var verifyTokenQueryKey = context.MergedJobDataMap.Get("verifyTokenQueryKey") as string;

        var verifyToken = verifyEmailService.GenerateVerifyToken(user!.Id);

        var email = fluentEmail.To(user.Email).Subject("Email megerősítése").Body(
            await razorViewToStringRenderer.RenderViewToStringAsync("/Views/Emails/VerifyEmail/VerifyEmail.cshtml",
                new VerifyEmailViewModel
                {
                    VerifyEmailUrl = $"{verifyUrl}?{verifyTokenQueryKey}={verifyToken}"
                }), true);

        await email.SendAsync();
    }
}