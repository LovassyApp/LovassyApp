namespace WebApi.Features.Auth.Models;

/// <summary>
///     The model representing the contents of a verify email token.
/// </summary>
public class VerifyEmailTokenContents : IUserTokenContents
{
    public string Purpose { get; set; }
    public Guid UserId { get; set; }
}