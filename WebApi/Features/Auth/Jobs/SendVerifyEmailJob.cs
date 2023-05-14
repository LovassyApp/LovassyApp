using System.Text.Json;
using FluentEmail.Core;
using Quartz;
using WebApi.Features.Auth.Services;
using WebApi.Infrastructure.Persistence.Entities;

namespace WebApi.Features.Auth.Jobs;

public class SendVerifyEmailJob : IJob
{
    private readonly IFluentEmail _fluentEmail;
    private readonly VerifyEmailService _verifyEmailService;

    public SendVerifyEmailJob(IFluentEmail fluentEmail, VerifyEmailService verifyEmailService)
    {
        _fluentEmail = fluentEmail;
        _verifyEmailService = verifyEmailService;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var user = JsonSerializer.Deserialize<User>((context.MergedJobDataMap.Get("userJson") as string)!);
        var verifyUrl = context.MergedJobDataMap.Get("verifyUrl") as string;
        var verifyTokenQueryKey = context.MergedJobDataMap.Get("verifyTokenQueryKey") as string;

        var verifyToken = _verifyEmailService.GenerateVerifyToken(user!.Id);

        var email = _fluentEmail.To(user.Email).Subject("Verify Email").Body(
            $"Please verify your email at: {verifyUrl}?{verifyTokenQueryKey}={verifyToken}");
        //TODO: Make an email template in razor and use that

        await email.SendAsync();
    }
}