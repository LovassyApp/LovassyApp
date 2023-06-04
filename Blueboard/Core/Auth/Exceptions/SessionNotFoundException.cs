using Blueboard.Core.Auth.Services;

namespace Blueboard.Core.Auth.Exceptions;

/// <summary>
///     The exception thrown when a session is not found in <see cref="SessionManager" />.
/// </summary>
public class SessionNotFoundException : Exception
{
}