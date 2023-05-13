namespace WebApi.Features.Auth.Services.Options;

public class RefreshOptions
{
    public int ExpiryDays { get; set; } = 30;
}