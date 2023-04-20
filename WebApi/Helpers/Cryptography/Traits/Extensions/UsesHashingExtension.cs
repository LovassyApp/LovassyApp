using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using WebApi.Helpers.Cryptography.Traits.Interfaces;

namespace WebApi.Helpers.Cryptography.Traits.Extensions;

public static class UsesHashingExtension
{
    public static string Hash(this IUsesHashing client, string data)
    {
        var dataBytes = Encoding.UTF8.GetBytes(data);

        var hashAlgorithm = SHA256.Create();
        var hashBytes = hashAlgorithm.ComputeHash(dataBytes);

        return Convert.ToBase64String(hashBytes);
    }

    public static bool Verify(this IUsesHashing client, string data, string hash)
    {
        var dataHash = Hash(client, data);

        return dataHash == hash;
    }

    public static string HashWithSalt(this IUsesHashing client, string data, string salt)
    {
        var dataBytes = Encoding.UTF8.GetBytes(data + ";" + salt);

        var hashAlgorithm = SHA512.Create();
        var hashBytes = hashAlgorithm.ComputeHash(dataBytes);

        return Convert.ToBase64String(hashBytes);
    }

    public static bool VerifyWithSalt(this IUsesHashing client, string data, string salt, string hash)
    {
        var dataHash = HashWithSalt(client, data, salt);

        return dataHash == hash;
    }

    public static string GenerateBasicKey(this IUsesHashing client, string data, string salt)
    {
        var keyBytes = KeyDerivation.Pbkdf2(data, Encoding.UTF8.GetBytes(salt), KeyDerivationPrf.HMACSHA512, 1000, 32);
        return Convert.ToBase64String(keyBytes);
    }

    public static string GenerateSalt(this IUsesHashing client)
    {
        var saltBytes = RandomNumberGenerator.GetBytes(128 / 8);
        return Convert.ToBase64String(saltBytes);
    }
}