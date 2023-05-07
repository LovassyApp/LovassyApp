using WebApi.Core.Cryptography.Services;

namespace WebApi.Core.Cryptography.Exceptions;

/// <summary>
///     The exception thrown when the salt in <see cref="HashManager" /> is trying to be accessed but is not set (likely
///     because the <see cref="HashManager" /> is not initialized).
/// </summary>
public class HasherSaltNotFoundException : Exception
{
}