using Blueboard.Core.Auth.Exceptions;
using Helpers.Cryptography.Utils;

namespace Blueboard.Core.Auth.Services;

/// <summary>
///     The singleton service responsible for storing the reset key password and encrypting and decrypting the master key
///     with it.
/// </summary>
public class ResetService
{
    private string? _resetKeyPassword;

    /// <summary>
    ///     Encrypts a given master key with the reset key password.
    /// </summary>
    /// <param name="masterKey">The master key to encrypt.</param>
    /// <returns></returns>
    /// <exception cref="ResetKeyPasswordMissingException">
    ///     The exception thrown when no reset key password has been set since
    ///     the application is running.
    /// </exception>
    public string EncryptMasterKey(string masterKey, string salt)
    {
        return EncryptionUtils.Encrypt(masterKey,
            HashingUtils.GenerateBasicKey(_resetKeyPassword ?? throw new ResetKeyPasswordMissingException(),
                salt));
    }

    /// <summary>
    ///     Decrypts a given master key with the reset key password.
    /// </summary>
    /// <param name="encryptedMasterKey">The encrypted master key as a string.</param>
    /// <returns>The decrypted master key.</returns>
    /// <exception cref="ResetKeyPasswordMissingException">
    ///     The exception thrown when no reset key password has been set since
    ///     the application is running.
    /// </exception>
    public string DecryptMasterKey(string encryptedMasterKey, string salt)
    {
        if (_resetKeyPassword == null)
            throw new ResetKeyPasswordMissingException();

        return EncryptionUtils.Decrypt(encryptedMasterKey,
            HashingUtils.GenerateBasicKey(_resetKeyPassword,
                salt));
    }

    /// <summary>
    ///     Sets the reset key password.
    /// </summary>
    /// <param name="resetKeyPassword">The password to which the reset key password will be set to.</param>
    public void SetResetKeyPassword(string? resetKeyPassword)
    {
        _resetKeyPassword = resetKeyPassword;
    }

    /// <summary>
    ///     Shows whether the reset key password has been set or not.
    /// </summary>
    /// <returns>Whether the reset key password has been set since the application is running or not.</returns>
    public bool IsResetKeyPasswordSet()
    {
        return _resetKeyPassword != null;
    }
}