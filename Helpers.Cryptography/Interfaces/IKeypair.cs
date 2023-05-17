namespace Helpers.Cryptography.Interfaces;

/// <summary>
///     An interface for a keypair that can be used for asymmetric encryption and decryption.
/// </summary>
public interface IKeypair
{
    public string PrivateKey { get; }
    public string PublicKey { get; }

    /// <summary>
    ///     Encrypts the given message with the public key.
    /// </summary>
    /// <param name="message">The message string to encrypt.</param>
    /// <returns>The encrypted message as a string.</returns>
    public string Encrypt(string message);

    /// <summary>
    ///     Decrypts the given encrypted message with the private key.
    /// </summary>
    /// <param name="encryptedMessage">The encrypted message as a string.</param>
    /// <returns>The decrypted message string.</returns>
    public string Decrypt(string encryptedMessage);
}