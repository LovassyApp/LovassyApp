using System.Text.Json;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Options;
using WebApi.Helpers.Cryptography.Services.Options;
using WebApi.Helpers.Cryptography.Utils;

namespace WebApi.Helpers.Cryptography.Services;

public class EncryptionService
{
    private readonly IDataProtector _dataProtector;

    public EncryptionService(IDataProtectionProvider dataProtectionProvider,
        IOptions<EncryptionOptions> encryptionOptions)
    {
        _dataProtector = dataProtectionProvider.CreateProtector(encryptionOptions.Value.DataProtectionPurpose);
    }

    /// <summary>
    ///     Wraps the <c>EncryptionUtils.Encrypt</c> method.
    /// </summary>
    /// <param name="data">The data to encrypt.</param>
    /// <param name="key">The key to encrypt the data with. Must be 32 bytes when converted from base64.</param>
    /// <returns>The encrypted data as a string.</returns>
    public string Encrypt(string data, string key)
    {
        return EncryptionUtils.Encrypt(data, key);
    }

    /// <summary>
    ///     Wraps the <c>EncryptionUtils.Decrypt</c> method.
    /// </summary>
    /// <param name="encryptedData">The encrypted data string.</param>
    /// <param name="key">The key used for encrypting the data. Must be 32 bytes when converted from base64.</param>
    /// <returns>The decrypted data as a string</returns>
    public string Decrypt(string encryptedData, string key)
    {
        return EncryptionUtils.Decrypt(encryptedData, key);
    }

    /// <summary>
    ///     Encrypts data using the Data Protection Api.
    /// </summary>
    /// <param name="data">The string data to encrypt.</param>
    /// <returns>The encrypted data as a string.</returns>
    public string Protect(string data)
    {
        return _dataProtector.Protect(data);
    }

    /// <summary>
    ///     Encrypts data with an expiration using the Data Protection Api.
    /// </summary>
    /// <param name="data">The string data to encrypt.</param>
    /// <param name="expiration">The time span in which the encrypted data expires and can no longer be decrypted.</param>
    /// <returns>The encrypted data as a string.</returns>
    public string Protect(string data, TimeSpan expiration)
    {
        var timeLimitedProtector = _dataProtector.ToTimeLimitedDataProtector();

        return timeLimitedProtector.Protect(data, expiration);
    }

    /// <summary>
    ///     Decrypts data using the Data Protection Api.
    /// </summary>
    /// <param name="encryptedData">The encrypted data as a string.</param>
    /// <returns>The decrypted string data.</returns>
    public string Unprotect(string encryptedData)
    {
        return _dataProtector.Unprotect(encryptedData);
    }

    /// <summary>
    ///     Decrypt expiring data using the Data Protection Api.
    /// </summary>
    /// <param name="encryptedData">The encrypted data string.</param>
    /// <param name="expiration">The <c>DateTimeOffset</c> in which the data expires.</param>
    /// <returns>The decrypted string data and also sets the expiration parameter.</returns>
    public string Unprotect(string encryptedData, out DateTimeOffset expiration)
    {
        var timeLimitedProtector = _dataProtector.ToTimeLimitedDataProtector();

        return timeLimitedProtector.Unprotect(encryptedData, out expiration);
    }

    /// <summary>
    ///     Serializes to json and encrypts data using the Data Protection Api.
    /// </summary>
    /// <param name="data">The object to serialize and encrypt.</param>
    /// <typeparam name="T">The type of the data.</typeparam>
    /// <returns>The encrypted data as a string.</returns>
    public string SerializeProtect<T>(T data)
    {
        var serializedData = JsonSerializer.Serialize(data);

        return _dataProtector.Protect(serializedData);
    }

    /// <summary>
    ///     Serializes to json and encrypts data with an expiration using the Data Protection Api.
    /// </summary>
    /// <param name="data">The object to serialize and encrypt.</param>
    /// <param name="expiration">The time span in which the data expires and can no longer be decrypted.</param>
    /// <typeparam name="T">The type of the data.</typeparam>
    /// <returns>The encrypted data as a string.</returns>
    public string SerializeProtect<T>(T data, TimeSpan expiration)
    {
        var serializedData = JsonSerializer.Serialize(data);
        var timeLimitedProtector = _dataProtector.ToTimeLimitedDataProtector();

        return timeLimitedProtector.Protect(serializedData, expiration);
    }

    /// <summary>
    ///     Decrypts and deserializes json data using the Data Protection Api.
    /// </summary>
    /// <param name="encryptedData">The encrypted data as a string</param>
    /// <typeparam name="T">The type of the object to which it deserializes.</typeparam>
    /// <returns>The decrypted and deserialized object.</returns>
    public T? DeserializeUnprotect<T>(string encryptedData)
    {
        var serializedData = _dataProtector.Unprotect(encryptedData);

        return JsonSerializer.Deserialize<T>(serializedData);
    }

    /// <summary>
    ///     Decrypts and deserializes expiring json data using the Data Protection Api.
    /// </summary>
    /// <param name="encryptedData">The encrypted data as a string.</param>
    /// <param name="expiration">The <c>DateTimeOffset</c> in which the data expires.</param>
    /// <typeparam name="T">The type of the object to which it deserializes.</typeparam>
    /// <returns>The decrypted and deserialized object.</returns>
    public T? DeserializeUnprotect<T>(string encryptedData, out DateTimeOffset expiration)
    {
        var timeLimitedProtector = _dataProtector.ToTimeLimitedDataProtector();
        var serializedData = timeLimitedProtector.Unprotect(encryptedData, out expiration);

        return JsonSerializer.Deserialize<T>(serializedData);
    }
}