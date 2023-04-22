using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace WebApi.Helpers.Cryptography.Utils;

public static class HashingUtils
{
    public static string Hash(string data)
    {
        var dataBytes = Encoding.UTF8.GetBytes(data);

        var hashAlgorithm = SHA256.Create();
        var hashBytes = hashAlgorithm.ComputeHash(dataBytes);

        return Convert.ToBase64String(hashBytes);
    }

    public static bool Verify(string data, string hash)
    {
        var dataHash = Hash(data);

        return dataHash == hash;
    }

    public static string HashWithSalt(string data, string salt)
    {
        var dataBytes = Encoding.UTF8.GetBytes(data + ";" + salt);

        var hashAlgorithm = SHA512.Create();
        var hashBytes = hashAlgorithm.ComputeHash(dataBytes);

        return Convert.ToBase64String(hashBytes);
    }

    public static bool VerifyWithSalt(string data, string salt, string hash)
    {
        var dataHash = HashWithSalt(data, salt);

        return dataHash == hash;
    }

    public static string GenerateBasicKey(string data, string salt)
    {
        var keyBytes = KeyDerivation.Pbkdf2(data, Encoding.UTF8.GetBytes(salt), KeyDerivationPrf.HMACSHA512, 1000, 32);
        return Convert.ToBase64String(keyBytes);
    }

    public static string GenerateSalt()
    {
        var saltBytes = RandomNumberGenerator.GetBytes(128 / 8);
        return Convert.ToBase64String(saltBytes);
    }
}