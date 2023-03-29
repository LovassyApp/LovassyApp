namespace WebApi.Helpers.Cryptography.Services;

public interface IHashService
{
    public string Hash(string data);
    public bool Verify(string data, string hash);
    public string HashWithSalt(string data, string salt);
    public bool VerifyWithSalt(string data, string salt, string hash);
    public string HashPassword(string password);
    public bool VerifyPassword(string password, string hash);
    public string GenerateBasicKey(string data, string salt);
}