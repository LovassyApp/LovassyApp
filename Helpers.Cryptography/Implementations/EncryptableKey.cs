using System.Security.Cryptography;
using Helpers.Cryptography.Exceptions;
using Helpers.Cryptography.Interfaces;
using Helpers.Cryptography.Utils;

namespace Helpers.Cryptography.Implementations;

/// <summary>
///     The implementation of <see cref="IEncryptableKey" /> using <see cref="EncryptionUtils" /> for locking and unlocking
///     the key.
/// </summary>
public class EncryptableKey : IEncryptableKey
{
    private string? _key;
    private string? _keyEncrypted;

    private bool _unlocked;

    public EncryptableKey(string? keyEncrypted = null)
    {
        if (keyEncrypted == null)
        {
            _key = GenerateRandomKey();
            _unlocked = true;
        }
        else
        {
            _keyEncrypted = keyEncrypted;
            _unlocked = false;
        }
    }

    /// <summary>
    ///     Locks (encrypts) the given key with the given password and salt.
    /// </summary>
    /// <param name="password">The secret with which the key is locked.</param>
    /// <param name="salt">The salt used for locking the key. (needed for extra security)</param>
    /// <throws cref="EncryptableKeyLockedException">The exception thrown when the key is already locked.</throws>
    /// <returns>The locked (encrypted) key as a string.</returns>
    public string Lock(string password, string salt)
    {
        if (!_unlocked)
            throw new EncryptableKeyLockedException();

        _unlocked = false;
        _keyEncrypted = EncryptionUtils.Encrypt(_key!, HashingUtils.GenerateBasicKey(password, salt));
        return _keyEncrypted;
    }

    /// <summary>
    ///     Unlocks (decrypts) the given key with the given password and salt.
    /// </summary>
    /// <param name="password">The secret with which the key was locked.</param>
    /// <param name="salt">The salt that was used for locking the key.</param>
    /// <throws cref="EncryptableKeyUnlockedException">The exception thrown when the key is already unlocked.</throws>
    /// <returns>The unlocked (decrypted) key.</returns>
    public string Unlock(string password, string salt)
    {
        if (_unlocked)
            throw new EncryptableKeyUnlockedException();

        _key = EncryptionUtils.Decrypt(_keyEncrypted!, HashingUtils.GenerateBasicKey(password, salt));
        _unlocked = true;
        return _key;
    }

    /// <summary>
    ///     If the key is unlocked, returns the key as a string.
    /// </summary>
    /// <throws cref="EncryptableKeyLockedException">The exception thrown when the key is locked.</throws>
    /// <returns>The unlocked key.</returns>
    public string GetKey()
    {
        if (!_unlocked)
            throw new EncryptableKeyLockedException();

        return _key!;
    }

    private static string GenerateRandomKey()
    {
        var keyBytes = RandomNumberGenerator.GetBytes(256 / 8);
        return Convert.ToBase64String(keyBytes);
    }
}