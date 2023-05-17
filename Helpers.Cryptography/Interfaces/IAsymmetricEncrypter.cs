namespace Helpers.Cryptography.Interfaces;

/// <summary>
///     The interface for an asymmetric encrypter which is only capable of encrypting and not decrypting but also only
///     requires a public key.
/// </summary>
public interface IAsymmetricEncrypter
{
    /// <summary>
    ///     Encrypts the given message with the public key.
    /// </summary>
    /// <param name="message">The message string to encrypt.</param>
    /// <returns>The encrypted message as a string.</returns>
    public string Encrypt(string message);
}