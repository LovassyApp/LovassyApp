using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using WebApi.Helpers.Cryptography.Exceptions;

namespace WebApi.Helpers.Cryptography.Models;

public class EncryptableKey : IEncryptableKey
{
    private string? _key;
    private string? _keyEncrypted;

    private bool _unlocked;

    public EncryptableKey(string? keyEncrypted)
    {
        if (keyEncrypted == null)
        {
            _key = GenerateRandomKey();
            _unlocked = true;
        }
        else
        {
            _keyEncrypted = keyEncrypted;
            _unlocked = false;
        }
    }

    public string Lock(string password, string salt)
    {
        if (!_unlocked)
            throw new EncryptableKeyLockedException();

        _keyEncrypted = Encrypt(_key!, GenerateBasicKey(password, salt));
        return _keyEncrypted;
    }

    public string Unlock(string password, string salt)
    {
        if (_unlocked)
            throw new EncryptableKeyUnlockedException();

        _key = Decrypt(_keyEncrypted!, GenerateBasicKey(password, salt));
        _unlocked = true;
        return _key;
    }

    public string GetKey()
    {
        if (!_unlocked)
            throw new EncryptableKeyUnlockedException();

        return _key!;
    }

    private static string GenerateRandomKey()
    {
        var keyBytes = RandomNumberGenerator.GetBytes(128 / 8);
        return Convert.ToBase64String(keyBytes);
    }

    private static string GenerateBasicKey(string data, string salt)
    {
        var keyBytes = KeyDerivation.Pbkdf2(data, Encoding.UTF8.GetBytes(salt), KeyDerivationPrf.HMACSHA512, 1000, 32);
        return Convert.ToBase64String(keyBytes);
    }

    private static string Encrypt(string data, string key)
    {
        var vector = GenerateIV();

        using var aesAlgorithm = Aes.Create();
        using var encryptor = aesAlgorithm.CreateEncryptor(Encoding.UTF8.GetBytes(key), vector);
        using var memoryStream = new MemoryStream();
        using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
        using (var streamWriter = new StreamWriter(cryptoStream))
        {
            streamWriter.Write(data);
        }

        var encryptedData = memoryStream.ToArray();

        return Convert.ToBase64String(vector) + ";" + Convert.ToBase64String(encryptedData);
    }

    private static string Decrypt(string encryptedData, string key)
    {
        var vector = Convert.FromBase64String(encryptedData.Split(';')[0]);
        var encryptedDataBytes = Convert.FromBase64String(encryptedData.Split(';')[1]);

        using var aesAlgorithm = Aes.Create();
        using var decryptor = aesAlgorithm.CreateDecryptor(Encoding.UTF8.GetBytes(key), vector);
        using var memoryStream = new MemoryStream(encryptedDataBytes);
        using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
        using var streamReader = new StreamReader(cryptoStream);

        return streamReader.ReadToEnd();
    }

    private static byte[] GenerateIV()
    {
        var aesAlgorithm = Aes.Create();
        aesAlgorithm.GenerateIV();

        return aesAlgorithm.IV;
    }
}