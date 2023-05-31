namespace WebApi.Features.Auth.Services.Options;

public class PasswordResetOptions
{
    public int ExpiryMinutes { get; set; } = 30;
}