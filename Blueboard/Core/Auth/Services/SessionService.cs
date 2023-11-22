using Blueboard.Core.Auth.Exceptions;
using Blueboard.Core.Auth.Models;
using Helpers.Cryptography.Utils;
using Microsoft.Extensions.Caching.Memory;

namespace Blueboard.Core.Auth.Services;

public class SessionService(IMemoryCache memoryCache)
{
    /// <summary>
    ///     Stops the session with the given token. For stopping the current session, use
    ///     <see cref="SessionManager.StopSession" />.
    /// </summary>
    /// <param name="token">The token for the session to stop.</param>
    /// <exception cref="ExternalSessionNotFoundException">The exception thrown when the session was not found.</exception>
    public void StopSession(string token)
    {
        var tokenHash = HashingUtils.Hash(token);
        var session = memoryCache.Get<Session>(tokenHash);

        if (session == null)
            return;

        memoryCache.Remove(session.Hash);
    }
}