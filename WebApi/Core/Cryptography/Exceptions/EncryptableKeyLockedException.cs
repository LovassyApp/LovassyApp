using WebApi.Core.Cryptography.Models;

namespace WebApi.Core.Cryptography.Exceptions;

/// <summary>
///     The exception thrown when an <see cref="EncryptableKey" /> is locked but its "internal key" is trying to be
///     accessed or when the <see cref="EncryptableKey" /> is trying to be locked but is already locked.
/// </summary>
public class EncryptableKeyLockedException : InvalidOperationException
{
}