using System.Security.Cryptography;
using WebApi.Helpers.Cryptography.Traits.Interfaces;

namespace WebApi.Helpers.Cryptography.Traits.Extensions;

public static class UsesEncryptionExtension
{
    public static string _Encrypt(this IUsesEncryption client, string data, string key)
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

    public static string _Decrypt(this IUsesEncryption client, string encryptedData, string key)
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