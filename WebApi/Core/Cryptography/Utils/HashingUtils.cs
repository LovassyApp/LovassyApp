using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Org.BouncyCastle.Security;

namespace WebApi.Core.Cryptography.Utils;

public static class HashingUtils
{
    /// <summary>
    ///     Hashes the provided data using SHA256.
    /// </summary>
    /// <param name="data">The string data to hash.</param>
    /// <returns>The hashed data as a string.</returns>
    public static string Hash(string data)
    {
        var dataBytes = Encoding.UTF8.GetBytes(data);

        var hashAlgorithm = SHA256.Create();
        var hashBytes = hashAlgorithm.ComputeHash(dataBytes);

        return Convert.ToBase64String(hashBytes);
    }

    /// <summary>
    ///     Verifies that the provided hash is the SHA256 hash of the provided data.
    /// </summary>
    /// <param name="data">The original data.</param>
    /// <param name="hash">The supposed hash of the original data.</param>
    /// <returns>Whether it is actually the hash of the provided data or not.</returns>
    public static bool Verify(string data, string hash)
    {
        var dataHash = Hash(data);

        return dataHash == hash;
    }

    /// <summary>
    ///     Hashes the given string data with a salt using SHA512.
    /// </summary>
    /// <param name="data">The data to hash.</param>
    /// <param name="salt">The salt to use for the hashing.</param>
    /// <returns>The hashed data as a string.</returns>
    public static string HashWithSalt(string data, string salt)
    {
        var dataBytes = Encoding.UTF8.GetBytes(data + ";" + salt);

        var hashAlgorithm = SHA512.Create();
        var hashBytes = hashAlgorithm.ComputeHash(dataBytes);

        return Convert.ToBase64String(hashBytes);
    }

    /// <summary>
    ///     Verifies that the provided hash is the SHA512 hash of the provided data and salt.
    /// </summary>
    /// <param name="data">The original data.</param>
    /// <param name="salt">The salt used for hashing the original data.</param>
    /// <param name="hash">The supposed hash of the original data.</param>
    /// <returns>Whether it is actually the hash of the provided data or not.</returns>
    public static bool VerifyWithSalt(string data, string salt, string hash)
    {
        var dataHash = HashWithSalt(data, salt);

        return dataHash == hash;
    }

    /// <summary>
    ///     Given any string as data and a salt, generates a key that can be used for encryption with the
    ///     <c>EncryptionService</c> or the <c>EncryptionUtils</c>.
    /// </summary>
    /// <param name="data">The string, intended to be kept as a secret, from which to generate the key.</param>
    /// <param name="salt">The salt used for generating the key.</param>
    /// <returns>The generated key as a base64 string.</returns>
    public static string GenerateBasicKey(string data, string salt)
    {
        var keyBytes = KeyDerivation.Pbkdf2(data, Encoding.UTF8.GetBytes(salt), KeyDerivationPrf.HMACSHA512, 1000, 32);
        return Convert.ToBase64String(keyBytes);
    }

    /// <summary>
    ///     Generates a random 16 byte salt.
    /// </summary>
    /// <returns>The base64 string representation of the generated salt.</returns>
    public static string GenerateSalt()
    {
        var secureRandom = new SecureRandom();
        var saltBytes = new byte[16];
        secureRandom.NextBytes(saltBytes);
        return Convert.ToBase64String(saltBytes);
    }
}