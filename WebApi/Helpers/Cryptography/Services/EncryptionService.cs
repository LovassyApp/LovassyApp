using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Options;
using WebApi.Helpers.Cryptography.Services.Options;

namespace WebApi.Helpers.Cryptography.Services;

public class EncryptionService : IEncryptionService
{
    private readonly IDataProtector _dataProtector;
    
    public EncryptionService(IDataProtectionProvider dataProtectionProvider, IOptions<EncryptionOptions> encryptionOptions)
    {
        _dataProtector = dataProtectionProvider.CreateProtector(encryptionOptions.Value.Purpose);
    }

    public string Encrypt(string data, string key)
    {
        byte[] vector = GenerateIV();

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

    public string Decrypt(string encryptedData, string key)
    {
        byte[] vector = Convert.FromBase64String(encryptedData.Split(';')[0]);
        byte[] encryptedDataBytes = Convert.FromBase64String(encryptedData.Split(';')[1]);
        
        using var aesAlgorithm = Aes.Create();
        using var decryptor = aesAlgorithm.CreateDecryptor(Encoding.UTF8.GetBytes(key), vector);
        using var memoryStream = new MemoryStream(encryptedDataBytes);
        using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
        using var streamReader = new StreamReader(cryptoStream);
        
        return streamReader.ReadToEnd();
    }

    public string Protect(string data)
    {
        return _dataProtector.Protect(data);
    }

    public string Unprotect(string encryptedData)
    {
        return _dataProtector.Unprotect(encryptedData);
    }

    public string SerializeProtect<T>(T data)
    {
        var serializedData = JsonSerializer.Serialize(data);
        
        return _dataProtector.Protect(serializedData);
    }

    public T? DeserializeUnprotect<T>(string encryptedData)
    {
        var serializedData = _dataProtector.Unprotect(encryptedData);
        
        return JsonSerializer.Deserialize<T>(serializedData);
    }
    
    private byte[] GenerateIV()
    {
        var aesAlgorithm = Aes.Create();
        aesAlgorithm.GenerateIV();
        
        return aesAlgorithm.IV;
    }
}