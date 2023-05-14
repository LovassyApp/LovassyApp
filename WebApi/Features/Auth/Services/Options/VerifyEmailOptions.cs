namespace WebApi.Features.Auth.Services.Options;

public class VerifyEmailOptions
{
    public int ExpiryMinutes { get; set; } = 30;
    public string FrontendUrl { get; set; }
    public string FrontendUrlQueryKey { get; set; } = "verifyToken";
}