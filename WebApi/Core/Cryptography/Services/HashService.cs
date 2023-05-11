using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Options;
using WebApi.Core.Cryptography.Services.Options;
using WebApi.Core.Cryptography.Utils;

namespace WebApi.Core.Cryptography.Services;

/// <summary>
///     The singleton service responsible for hashing passwords and other data using the <see cref="HashingUtils" /> class.
/// </summary>
public class HashService
{
    private readonly HashOptions _hashOptions;
    private readonly RandomNumberGenerator _rng;

    public HashService(IOptions<HashOptions> hashOptions)
    {
        _rng = hashOptions.Value.DefaultRng;

        if (hashOptions.Value.PasswordHashIterations < 1)
            throw new ArgumentException("Password hash iterations must be greater than 0", nameof(hashOptions));

        if (hashOptions.Value.PasswordSaltLength < 1)
            throw new ArgumentException("Password salt length must be greater than 0", nameof(hashOptions));

        if (hashOptions.Value.PasswordBytesRequested < 1)
            throw new ArgumentException("Password bytes requested must be greater than 0", nameof(hashOptions));

        _hashOptions = hashOptions.Value;
    }

    /// <summary>
    ///     A wrapper around the <see cref="HashingUtils.Hash" /> method.
    /// </summary>
    /// <param name="data">The string data to hash.</param>
    /// <returns>The hashed data as a string.</returns>
    public string Hash(string data)
    {
        return HashingUtils.Hash(data);
    }

    /// <summary>
    ///     A wrapper around the <see cref="HashingUtils.Verify" /> method.
    /// </summary>
    /// <param name="data">The original data.</param>
    /// <param name="hash">The supposed hash of the original data.</param>
    /// <returns>Whether it is actually the hash of the provided data or not.</returns>
    public bool Verify(string data, string hash)
    {
        return HashingUtils.Verify(data, hash);
    }

    /// <summary>
    ///     A wrapper around the <see cref="HashingUtils.HashWithSalt" />  method.
    /// </summary>
    /// <param name="data">The data to hash.</param>
    /// <param name="salt">The salt to use for the hashing.</param>
    /// <returns>The hashed data as a string.</returns>
    public string HashWithSalt(string data, string salt)
    {
        return HashingUtils.HashWithSalt(data, salt);
    }

    /// <summary>
    ///     A wrapper around the <see cref="HashingUtils.VerifyWithSalt" />  method.
    /// </summary>
    /// <param name="data">The original data.</param>
    /// <param name="salt">The salt used for hashing the original data.</param>
    /// <param name="hash">The supposed hash of the original data.</param>
    /// <returns>Whether it is actually the hash of the provided data or not.</returns>
    public bool VerifyWithSalt(string data, string salt, string hash)
    {
        return HashingUtils.VerifyWithSalt(data, salt, hash);
    }

    /// <summary>
    ///     A wrapper around the <see cref="HashingUtils.GenerateBasicKey" />  method.
    /// </summary>
    /// <param name="data">The string, intended to be kept as a secret, from which to generate the key.</param>
    /// <param name="salt">The salt used for generating the key.</param>
    /// <returns>The generated key as a base64 string.</returns>
    public string GenerateBasicKey(string data, string salt)
    {
        return HashingUtils.GenerateBasicKey(data, salt);
    }

    /// <summary>
    ///     A wrapper around the <see cref="HashingUtils.GenerateSalt" />  method.
    /// </summary>
    /// <returns>The generated salt.</returns>
    public string GenerateSalt()
    {
        return HashingUtils.GenerateSalt();
    }

    /// <summary>
    ///     Hashes a password using PBKDF2 with HMAC-SHA512. It does the exact same thing as Identity Core.
    /// </summary>
    /// <param name="password">The password to hash.</param>
    /// <returns>The hashed password as a string.</returns>
    public string HashPassword(string password)
    {
        var salt = new byte[_hashOptions.PasswordSaltLength];
        _rng.GetBytes(salt);

        var subkey = KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA512,
            _hashOptions.PasswordHashIterations,
            _hashOptions.PasswordBytesRequested);

        var outputBytes = new byte[13 + salt.Length + subkey.Length];
        outputBytes[0] = 0x01;
        WriteNetworkByteOrder(outputBytes, 1, (uint)KeyDerivationPrf.HMACSHA512);
        WriteNetworkByteOrder(outputBytes, 5, (uint)_hashOptions.PasswordHashIterations);
        WriteNetworkByteOrder(outputBytes, 9, (uint)_hashOptions.PasswordSaltLength);
        Buffer.BlockCopy(salt, 0, outputBytes, 13, salt.Length);
        Buffer.BlockCopy(subkey, 0, outputBytes, 13 + _hashOptions.PasswordSaltLength, subkey.Length);

        return Convert.ToBase64String(outputBytes);
    }

    /// <summary>
    ///     Verifies a password against a hash using PBKDF2. It does the exact same thing as Identity Core.
    /// </summary>
    /// <param name="password">The password to verify.</param>
    /// <param name="hash">The supposed hash.</param>
    /// <returns>Whether it's actually the hash of the given password not.</returns>
    public bool VerifyPassword(string password, string hash)
    {
        var decodedHash = Convert.FromBase64String(hash);

        if (decodedHash.Length == 0)
            return false;

        if (decodedHash[0] != 0x01)
            return false;

        var prf = (KeyDerivationPrf)ReadNetworkByteOrder(decodedHash, 1);
        var iterCount = (int)ReadNetworkByteOrder(decodedHash, 5);
        var saltLength = (int)ReadNetworkByteOrder(decodedHash, 9);

        if (saltLength != _hashOptions.PasswordSaltLength)
            return false;

        if (iterCount < 1)
            return false;

        var salt = new byte[saltLength];
        Buffer.BlockCopy(decodedHash, 13, salt, 0, salt.Length);

        var expectedSubkey = new byte[decodedHash.Length - 13 - salt.Length];
        Buffer.BlockCopy(decodedHash, 13 + salt.Length, expectedSubkey, 0, expectedSubkey.Length);

        var actualSubkey = KeyDerivation.Pbkdf2(password, salt, prf, iterCount, expectedSubkey.Length);

        return CryptographicOperations.FixedTimeEquals(actualSubkey, expectedSubkey);
    }

    private static uint ReadNetworkByteOrder(byte[] buffer, int offset)
    {
        return ((uint)buffer[offset + 0] << 24)
               | ((uint)buffer[offset + 1] << 16)
               | ((uint)buffer[offset + 2] << 8)
               | buffer[offset + 3];
    }

    private static void WriteNetworkByteOrder(byte[] buffer, int offset, uint value)
    {
        buffer[offset + 0] = (byte)(value >> 24);
        buffer[offset + 1] = (byte)(value >> 16);
        buffer[offset + 2] = (byte)(value >> 8);
        buffer[offset + 3] = (byte)(value >> 0);
    }
}