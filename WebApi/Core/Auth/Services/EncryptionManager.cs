using System.Text.Json;
using Helpers.Cryptography.Services.Options;
using Helpers.Cryptography.Utils;
using Microsoft.Extensions.Options;
using WebApi.Core.Auth.Exceptions;
using WebApi.Infrastructure.Persistence.Entities;

namespace WebApi.Core.Auth.Services;

/// <summary>
///     The scoped service responsible for encrypting and decrypting data using the master key of the current
///     <see cref="User" />.
/// </summary>
public class EncryptionManager
{
    private readonly EncryptionOptions _encryptionOptions;
    private readonly SessionManager _sessionManager;

    private string? _masterKey;

    public EncryptionManager(SessionManager sessionManager, IOptions<EncryptionOptions> encryptionOptions)
    {
        _sessionManager = sessionManager;
        _encryptionOptions = encryptionOptions.Value;
    }

    public string? MasterKey
    {
        get => _masterKey;
        set
        {
            if (_sessionManager.Session == null)
                throw new SessionNotFoundException();

            _masterKey = value;
            _sessionManager.SetEncrypted(_encryptionOptions.MasterKeySessionKey, value);
        }
    }

    /// <summary>
    ///     Sets the master key temporarily (will not be saved in the session, to save it in
    ///     the session set it through <see cref="MasterKey" />).
    /// </summary>
    /// <param name="key">The master key to be used.</param>
    public void SetMasterKeyTemporarily(string key)
    {
        _masterKey = key;
    }

    /// <summary>
    ///     Encrypts the data using the master key of the current <see cref="User" />.
    /// </summary>
    /// <param name="data">The string data to encrypt.</param>
    /// <returns>The encrypted data string.</returns>
    public string Encrypt(string data)
    {
        if (_masterKey == null)
            Init();

        return EncryptionUtils.Encrypt(data, _masterKey);
    }

    /// <summary>
    ///     Decrypts the given string data using the master key of the current <see cref="User" />.
    /// </summary>
    /// <param name="encryptedData">The encrypted string data.</param>
    /// <returns>The decrypted string value.</returns>
    public string Decrypt(string encryptedData)
    {
        if (_masterKey == null)
            Init();

        return EncryptionUtils.Decrypt(encryptedData, _masterKey);
    }


    /// <summary>
    ///     Serializes to json and encrypts the given data using the master key of the current <see cref="User" />.
    /// </summary>
    /// <param name="data">The data to encrypt.</param>
    /// <typeparam name="T">The type of the data to encrypt.</typeparam>
    /// <returns>The encrypted data string.</returns>
    public string SerializeEncrypt<T>(T data)
    {
        if (_masterKey == null)
            Init();

        var serializedData = JsonSerializer.Serialize(data);

        return EncryptionUtils.Encrypt(serializedData, _masterKey);
    }

    /// <summary>
    ///     Decrypts the given data using the master key of the current <see cref="User" /> and deserializes it to the given
    ///     type.
    /// </summary>
    /// <param name="encryptedData">The encrypted data as a string.</param>
    /// <typeparam name="T">The type of the object to deserialize to.</typeparam>
    /// <returns>The decrypted and deserialized object.</returns>
    public T? DeserializeDecrypt<T>(string encryptedData)
    {
        if (_masterKey == null)
            Init();

        var serializedData = EncryptionUtils.Decrypt(encryptedData, _masterKey);

        return JsonSerializer.Deserialize<T>(serializedData);
    }

    private void Init()
    {
        try
        {
            _masterKey = _sessionManager.GetEncrypted(_encryptionOptions.MasterKeySessionKey);
        }
        catch (SessionNotFoundException)
        {
            throw
                new MasterKeyNotFoundException(); // That's some weird shit going on here but ig it's not that big of a deal as long as you have a trace
        }

        if (_masterKey == null)
            throw new MasterKeyNotFoundException();
    }
}