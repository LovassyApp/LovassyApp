using Blueboard.Features.Auth.Interfaces;

namespace Blueboard.Features.Auth.Models;

public class PasswordResetTokenContents : IUserTokenContents
{
    public string Purpose { get; set; }
    public Guid UserId { get; set; }
}