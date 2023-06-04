using System.Text.Json;
using Blueboard.Features.Auth.Services;
using Blueboard.Infrastructure.Persistence.Entities;
using FluentEmail.Core;
using Quartz;

namespace Blueboard.Features.Auth.Jobs;

public class SendPasswordResetJob : IJob
{
    private readonly IFluentEmail _fluentEmail;
    private readonly PasswordResetService _passwordResetService;

    public SendPasswordResetJob(IFluentEmail fluentEmail, PasswordResetService passwordResetService)
    {
        _fluentEmail = fluentEmail;
        _passwordResetService = passwordResetService;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var user = JsonSerializer.Deserialize<User>((context.MergedJobDataMap.Get("userJson") as string)!);
        var passwordResetUrl = context.MergedJobDataMap.Get("passwordResetUrl") as string;
        var passwordResetTokenQueryKey = context.MergedJobDataMap.Get("passwordResetTokenQueryKey") as string;

        var passwordResetToken = _passwordResetService.GeneratePasswordResetToken(user!.Id);

        var email = _fluentEmail.To(user.Email).Subject("Reset Password").Body(
            $"You can reset your password at: {passwordResetUrl}?{passwordResetTokenQueryKey}={passwordResetToken}");
        //TODO: Make an email template in razor and use that

        await email.SendAsync();
    }
}