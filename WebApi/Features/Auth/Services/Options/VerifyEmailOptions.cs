namespace WebApi.Features.Auth.Services.Options;

public class VerifyEmailOptions
{
    public int ExpiryMinutes { get; set; } = 30;
}