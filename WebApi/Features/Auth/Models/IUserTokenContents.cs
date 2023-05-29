using WebApi.Infrastructure.Persistence.Entities;

namespace WebApi.Features.Auth.Models;

/// <summary>
///     The interface for all token contents, which belong to a <see cref="User" />.
/// </summary>
public interface IUserTokenContents
{
    public string Purpose { get; set; }
    public Guid UserId { get; set; }
}