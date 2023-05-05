namespace WebApi.Core.Cryptography.Models;

public interface IAsymmetricEncrypter
{
    /// <summary>
    ///     Encrypts the given message with the public key.
    /// </summary>
    /// <param name="message">The message string to encrypt.</param>
    /// <returns>The encrypted message as a string.</returns>
    public string Encrypt(string message);
}