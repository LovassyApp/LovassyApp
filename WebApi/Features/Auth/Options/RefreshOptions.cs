namespace WebApi.Features.Auth.Options;

public class RefreshOptions
{
    public int ExpiryDays { get; set; } = 30;
}