using Blueboard.Infrastructure.Persistence.Entities;

namespace Blueboard.Features.Auth.Interfaces;

/// <summary>
///     The interface for all token contents, which belong to a <see cref="User" />.
/// </summary>
public interface IUserTokenContents
{
    public string Purpose { get; set; }
    public Guid UserId { get; set; }
}