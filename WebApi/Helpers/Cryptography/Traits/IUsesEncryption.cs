using System.Security.Cryptography;

namespace WebApi.Helpers.Cryptography.Traits;

public interface IUsesEncryption
{
    public string _Encrypt(string data, string key)
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

    public string _Decrypt(string encryptedData, string key)
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