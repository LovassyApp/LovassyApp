using System.Security.Cryptography;

namespace WebApi.Core.Cryptography.Utils;

public static class EncryptionUtils
{
    /// <summary>
    ///     Encrypts the given string data with the given key using AES.
    /// </summary>
    /// <param name="data">The string data to encrypt.</param>
    /// <param name="key">The key to use for encrypting. Must be 32 bytes when converted from base64.</param>
    /// <returns>The encrypted data as a string.</returns>
    public static string Encrypt(string data, string key)
    {
        var vector = _GenerateIV();

        using var aesAlgorithm = Aes.Create();
        using var encryptor = aesAlgorithm.CreateEncryptor(Convert.FromBase64String(key), vector);
        using var memoryStream = new MemoryStream();
        using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
        using (var streamWriter = new StreamWriter(cryptoStream))
        {
            streamWriter.Write(data);
        }

        var encryptedData = memoryStream.ToArray();

        return Convert.ToBase64String(vector) + ";" + Convert.ToBase64String(encryptedData);
    }

    /// <summary>
    ///     Decrypts the given encrypted data with the given key using AES.
    /// </summary>
    /// <param name="encryptedData">The encrypted data as a string.</param>
    /// <param name="key">The key used for encrypting the data. Must be 32 bytes when converted from base64.</param>
    /// <returns>The decrypted string data.</returns>
    public static string Decrypt(string encryptedData, string key)
    {
        var vector = Convert.FromBase64String(encryptedData.Split(';')[0]);
        var encryptedDataBytes = Convert.FromBase64String(encryptedData.Split(';')[1]);

        using var aesAlgorithm = Aes.Create();
        using var decryptor = aesAlgorithm.CreateDecryptor(Convert.FromBase64String(key), vector);
        using var memoryStream = new MemoryStream(encryptedDataBytes);
        using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
        using var streamReader = new StreamReader(cryptoStream);

        return streamReader.ReadToEnd();
    }

    private static byte[] _GenerateIV()
    {
        var aesAlgorithm = Aes.Create();
        aesAlgorithm.GenerateIV();

        return aesAlgorithm.IV;
    }
}