namespace WebApi.Helpers.Cryptography.Models;

public interface IEncryptableKey
{
    public string Lock(string password, string salt);
    public string Unlock(string password, string salt);
    public string GetKey();
}