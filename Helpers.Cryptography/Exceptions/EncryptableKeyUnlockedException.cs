using Helpers.Cryptography.Implementations;

namespace Helpers.Cryptography.Exceptions;

/// <summary>
///     The exception thrown when an <see cref="EncryptableKey" /> is trying to be unlocked but is already unlocked.
/// </summary>
public class EncryptableKeyUnlockedException : InvalidOperationException
{
}