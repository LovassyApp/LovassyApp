namespace WebApi.Features.Auth.Models;

public class PasswordResetTokenContents : IUserTokenContents
{
    public string Purpose { get; set; }
    public Guid UserId { get; set; }
}