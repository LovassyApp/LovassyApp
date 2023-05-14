namespace WebApi.Features.Auth.Models;

public class RefreshTokenContents : IUserTokenContents
{
    public string Password { get; set; }
    public string Purpose { get; set; }
    public Guid UserId { get; set; }
}