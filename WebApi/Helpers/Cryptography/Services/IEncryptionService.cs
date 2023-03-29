namespace WebApi.Helpers.Cryptography.Services;

public interface IEncryptionService
{
    public string Encrypt(string data, string key);
    public string Decrypt(string encryptedData, string key);
    public string Protect(string data);
    public string Protect(string data, TimeSpan expiration);
    public string Unprotect(string encryptedData);
    public string Unprotect(string encryptedData, out DateTimeOffset expiration);
    public string SerializeProtect<T>(T data);
    public string SerializeProtect<T>(T data, TimeSpan expiration);
    public T? DeserializeUnprotect<T>(string encryptedData);
    public T? DeserializeUnprotect<T>(string encryptedData, out DateTimeOffset expiration);
}