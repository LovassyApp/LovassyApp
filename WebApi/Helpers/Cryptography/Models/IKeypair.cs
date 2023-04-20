namespace WebApi.Helpers.Cryptography.Models;

public interface IKeypair
{
    public string PrivateKey { get; }
    public string PublicKey { get; }

    public string Encrypt(string message);
    public string Decrypt(string encryptedMessage);
}