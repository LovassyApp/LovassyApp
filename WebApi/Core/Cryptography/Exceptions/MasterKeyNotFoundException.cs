using WebApi.Core.Cryptography.Services;

namespace WebApi.Core.Cryptography.Exceptions;

/// <summary>
///     The exception thrown when the master key in <see cref="EncryptionManager" /> is trying to be accessed but is not
///     set (likely because the <see cref="EncryptionManager" /> is not initialized).
/// </summary>
public class MasterKeyNotFoundException : Exception
{
}