using WebApi.Core.Cryptography.Services;

namespace WebApi.Core.Cryptography.Exceptions;

/// <summary>
///     The exception thrown when the reset key password in <see cref="ResetService" /> is trying to be accessed but is not
///     set.
/// </summary>
public class ResetKeyPasswordMissingException : InvalidOperationException
{
}