using Blueboard.Infrastructure.Persistence.Entities;

namespace Blueboard.Core.Auth.Models;

/// <summary>
///     The model representing a session. It is only ever stored in the cache. The <see cref="Session.Data" /> property is
///     what contains the master key of the <see cref="User" /> in an encrypted form.
/// </summary>
public class Session
{
    public Dictionary<string, string> Data { get; set; }

    public string Hash { get; set; }
    public string Salt { get; set; }

    public DateTime Expiry { get; set; }

    public SessionAccessToken AccessToken { get; set; }
}

/// <summary>
///     The model representing an access token associated with a <see cref="Session" />. Basically a stripped down version
///     of the <see cref="PersonalAccessToken" />.
/// </summary>
public class SessionAccessToken
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
}