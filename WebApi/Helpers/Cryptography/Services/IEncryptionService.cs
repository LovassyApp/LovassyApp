namespace WebApi.Helpers.Cryptography.Services;

public interface IEncryptionService
{
    public string Encrypt(string data, string key);
    public string Decrypt(string encryptedData, string key);
    public string Protect(string data);
    public string Unprotect(string encryptedData);
    public string SerializeProtect<T>(T data);
    public T? DeserializeUnprotect<T>(string encryptedData);
}