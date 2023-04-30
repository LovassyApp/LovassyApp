namespace WebApi.Core.Auth.Models;

public class RefreshTokenContents
{
    public string Password { get; set; }
    public Guid UserId { get; set; }
}