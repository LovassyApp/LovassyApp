using Blueboard.Core.Auth.Services;

namespace Blueboard.Core.Auth.Exceptions;

/// <summary>
///     The exception thrown when the reset key password in <see cref="ResetService" /> is trying to be accessed but is not
///     set.
/// </summary>
public class ResetKeyPasswordMissingException : InvalidOperationException
{
}