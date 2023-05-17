namespace Helpers.Cryptography.Interfaces;

/// <summary>
///     An interface for a symmetric encryption key that can be locked and unlocked.
/// </summary>
public interface IEncryptableKey
{
    /// <summary>
    ///     Locks (encrypts) the given key with the given password and salt.
    /// </summary>
    /// <param name="password">The secret with which the key is locked.</param>
    /// <param name="salt">The salt used for locking the key. (needed for extra security)</param>
    /// <returns>The locked (encrypted) key as a string.</returns>
    public string Lock(string password, string salt);

    /// <summary>
    ///     Unlocks (decrypts) the given key with the given password and salt.
    /// </summary>
    /// <param name="password">The secret with which the key was locked.</param>
    /// <param name="salt">The salt that was used for locking the key.</param>
    /// <returns>The unlocked (decrypted) key.</returns>
    public string Unlock(string password, string salt);

    /// <summary>
    ///     If the key is unlocked, returns the key as a string. Otherwise, may throw an exception.
    /// </summary>
    /// <returns>The unlocked key.</returns>
    public string GetKey();
}