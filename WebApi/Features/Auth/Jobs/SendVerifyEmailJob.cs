using System.Text.Json;
using FluentEmail.Core;
using Microsoft.Extensions.Options;
using Quartz;
using WebApi.Features.Auth.Services;
using WebApi.Features.Auth.Services.Options;
using WebApi.Infrastructure.Persistence.Entities;

namespace WebApi.Features.Auth.Jobs;

public class SendVerifyEmailJob : IJob
{
    private readonly IFluentEmail _fluentEmail;
    private readonly VerifyEmailOptions _verifyEmailOptions;
    private readonly VerifyEmailService _verifyEmailService;

    public SendVerifyEmailJob(IFluentEmail fluentEmail, VerifyEmailService verifyEmailService,
        IOptions<VerifyEmailOptions> verifyEmailOptions)
    {
        _fluentEmail = fluentEmail;
        _verifyEmailService = verifyEmailService;
        _verifyEmailOptions = verifyEmailOptions.Value;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var user = JsonSerializer.Deserialize<User>((context.MergedJobDataMap.Get("userJson") as string)!);

        var verifyToken = _verifyEmailService.GenerateVerifyToken(user!.Id);

        var email = _fluentEmail.To(user.Email).Subject("Verify Email").Body(
            $"Please verify your email at: {_verifyEmailOptions.FrontendUrl}?{_verifyEmailOptions.FrontendUrlQueryKey}={verifyToken}");
        //TODO: Make an email template in razor and use that

        await email.SendAsync();
    }
}