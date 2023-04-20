using System.Security.Cryptography;
using System.Text.Json;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Options;
using WebApi.Helpers.Cryptography.Services.Options;
using WebApi.Helpers.Cryptography.Traits.Interfaces;

namespace WebApi.Helpers.Cryptography.Services;

public class EncryptionService : IUsesEncryption
{
    private readonly IDataProtector _dataProtector;

    public EncryptionService(IDataProtectionProvider dataProtectionProvider,
        IOptions<EncryptionOptions> encryptionOptions)
    {
        _dataProtector = dataProtectionProvider.CreateProtector(encryptionOptions.Value.DataProtectionPurpose);
    }

    public string Protect(string data)
    {
        return _dataProtector.Protect(data);
    }

    public string Protect(string data, TimeSpan expiration)
    {
        var timeLimitedProtector = _dataProtector.ToTimeLimitedDataProtector();

        return timeLimitedProtector.Protect(data, expiration);
    }

    public string Unprotect(string encryptedData)
    {
        return _dataProtector.Unprotect(encryptedData);
    }

    public string Unprotect(string encryptedData, out DateTimeOffset expiration)
    {
        var timeLimitedProtector = _dataProtector.ToTimeLimitedDataProtector();

        return timeLimitedProtector.Unprotect(encryptedData, out expiration);
    }

    public string SerializeProtect<T>(T data)
    {
        var serializedData = JsonSerializer.Serialize(data);

        return _dataProtector.Protect(serializedData);
    }

    public string SerializeProtect<T>(T data, TimeSpan expiration)
    {
        var serializedData = JsonSerializer.Serialize(data);
        var timeLimitedProtector = _dataProtector.ToTimeLimitedDataProtector();

        return timeLimitedProtector.Protect(serializedData, expiration);
    }

    public T? DeserializeUnprotect<T>(string encryptedData)
    {
        var serializedData = _dataProtector.Unprotect(encryptedData);

        return JsonSerializer.Deserialize<T>(serializedData);
    }

    public T? DeserializeUnprotect<T>(string encryptedData, out DateTimeOffset expiration)
    {
        var timeLimitedProtector = _dataProtector.ToTimeLimitedDataProtector();
        var serializedData = timeLimitedProtector.Unprotect(encryptedData, out expiration);

        return JsonSerializer.Deserialize<T>(serializedData);
    }

    private static byte[] GenerateIV()
    {
        var aesAlgorithm = Aes.Create();
        aesAlgorithm.GenerateIV();

        return aesAlgorithm.IV;
    }
}