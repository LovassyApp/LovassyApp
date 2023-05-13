using WebApi.Core.Auth.Services;
using WebApi.Core.Cryptography.Services;

namespace WebApi.Core.Cryptography.Exceptions;

/// <summary>
///     The exception thrown when the master key in <see cref="EncryptionManager" /> is trying to be accessed but is not
///     set by <see cref="EncryptionManager.SetMasterKeyTemporarily" /> and is not set in the <see cref="SessionManager" />
///     either.
/// </summary>
public class MasterKeyNotFoundException : Exception
{
}