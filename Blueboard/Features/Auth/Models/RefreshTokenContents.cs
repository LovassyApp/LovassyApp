using Blueboard.Features.Auth.Interfaces;

namespace Blueboard.Features.Auth.Models;

/// <summary>
///     The model representing the contents of a refresh token.
/// </summary>
public class RefreshTokenContents : IUserTokenContents
{
    public string Password { get; set; }
    public string Purpose { get; set; }
    public Guid UserId { get; set; }
}