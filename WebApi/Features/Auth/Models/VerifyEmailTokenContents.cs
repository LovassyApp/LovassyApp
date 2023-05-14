namespace WebApi.Features.Auth.Models;

public class VerifyEmailTokenContents : IUserTokenContents
{
    public string Purpose { get; set; }
    public Guid UserId { get; set; }
}