using WebApi.Infrastructure.Persistence.Entities;

namespace WebApi.Core.Auth.Services;

/// <summary>
///     The scoped service which provides access to the current <see cref="User" />, if one is available. (The endpoint/hub
///     requires authentication)
/// </summary>
public class UserAccessor
{
    public User? User { get; set; }
}