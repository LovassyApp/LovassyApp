using System.Text.Json;
using Blueboard.Features.Auth.Services;
using Blueboard.Infrastructure.Persistence.Entities;
using FluentEmail.Core;
using Helpers.Email.Services;
using Helpers.Email.Views.Emails.PasswordReset;
using Quartz;

namespace Blueboard.Features.Auth.Jobs;

public class SendPasswordResetJob : IJob
{
    private readonly IFluentEmail _fluentEmail;
    private readonly PasswordResetService _passwordResetService;
    private readonly RazorViewToStringRenderer _razorViewToStringRenderer;

    public SendPasswordResetJob(IFluentEmail fluentEmail, PasswordResetService passwordResetService,
        RazorViewToStringRenderer razorViewToStringRenderer)
    {
        _fluentEmail = fluentEmail;
        _passwordResetService = passwordResetService;
        _razorViewToStringRenderer = razorViewToStringRenderer;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var user = JsonSerializer.Deserialize<User>((context.MergedJobDataMap.Get("userJson") as string)!);
        var passwordResetUrl = context.MergedJobDataMap.Get("passwordResetUrl") as string;
        var passwordResetTokenQueryKey = context.MergedJobDataMap.Get("passwordResetTokenQueryKey") as string;

        var passwordResetToken = _passwordResetService.GeneratePasswordResetToken(user!.Id);

        var email = _fluentEmail.To(user.Email).Subject("Reset Password").Body(
            await _razorViewToStringRenderer.RenderViewToStringAsync("/Views/Emails/PasswordReset/PasswordReset.cshtml",
                new PasswordResetViewModel
                {
                    PasswordResetUrl = $"{passwordResetUrl}?{passwordResetTokenQueryKey}={passwordResetToken}"
                }), true);

        await email.SendAsync();
    }
}