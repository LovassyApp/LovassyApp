using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace WebApi.Helpers.Cryptography.Traits;

public interface IUsesHashing
{
    public string _Hash(string data)
    {
        var dataBytes = Encoding.UTF8.GetBytes(data);

        var hashAlgorithm = SHA256.Create();
        var hashBytes = hashAlgorithm.ComputeHash(dataBytes);

        return Convert.ToBase64String(hashBytes);
    }

    public bool _Verify(string data, string hash)
    {
        var dataHash = _Hash(data);

        return dataHash == hash;
    }

    public string _HashWithSalt(string data, string salt)
    {
        var dataBytes = Encoding.UTF8.GetBytes(data + ";" + salt);

        var hashAlgorithm = SHA512.Create();
        var hashBytes = hashAlgorithm.ComputeHash(dataBytes);

        return Convert.ToBase64String(hashBytes);
    }

    public bool _VerifyWithSalt(string data, string salt, string hash)
    {
        var dataHash = _HashWithSalt(data, salt);

        return dataHash == hash;
    }

    public string _GenerateBasicKey(string data, string salt)
    {
        var keyBytes = KeyDerivation.Pbkdf2(data, Encoding.UTF8.GetBytes(salt), KeyDerivationPrf.HMACSHA512, 1000, 32);
        return Convert.ToBase64String(keyBytes);
    }

    public string _GenerateSalt()
    {
        var saltBytes = RandomNumberGenerator.GetBytes(128 / 8);
        return Convert.ToBase64String(saltBytes);
    }
}