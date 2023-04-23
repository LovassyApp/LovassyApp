using System.Text.Json;
using Microsoft.Extensions.Options;
using WebApi.Core.Auth.Exceptions;
using WebApi.Core.Auth.Services;
using WebApi.Core.Cryptography.Exceptions;
using WebApi.Core.Cryptography.Services.Options;
using WebApi.Core.Cryptography.Utils;

namespace WebApi.Core.Cryptography.Services;

public class EncryptionManager
{
    private readonly string _masterKeySessionKey;
    private readonly SessionManager _sessionManager;

    private string? _masterKey;

    public EncryptionManager(SessionManager sessionManager, IOptions<EncryptionOptions> encryptionOptions)
    {
        _sessionManager = sessionManager;
        _masterKeySessionKey = encryptionOptions.Value.MasterKeySessionKey;
    }

    public string? MasterKey
    {
        get => _masterKey;
        set
        {
            if (_sessionManager.Session == null)
                throw new SessionNotFoundException();

            _masterKey = value;
            _sessionManager.SetEncrypted(_masterKeySessionKey, value);
        }
    }

    /// <summary>
    ///     Initializes the encryption manager with a master key. If no key is provided, it tries to get it from the session.
    /// </summary>
    /// <param name="key">The master key to be used.</param>
    /// <exception cref="SessionNotFoundException">
    ///     The exception thrown when there is no key provided but the session manager
    ///     has not been initialized yet.
    /// </exception>
    /// <exception cref="MasterKeyNotFoundException">
    ///     The exception thrown when no key was provided and the master key is not
    ///     stored in the current session.
    /// </exception>
    public void Init(string? key = null)
    {
        if (key != null)
        {
            _masterKey = key;
        }
        else
        {
            if (_sessionManager.Session == null)
                throw new SessionNotFoundException();

            _masterKey = _sessionManager.GetEncrypted(_masterKeySessionKey);

            if (_masterKey == null)
                throw new MasterKeyNotFoundException();
        }
    }

    /// <summary>
    ///     Encrypts the data using the master key of the current user.
    /// </summary>
    /// <param name="data">The string data to encrypt.</param>
    /// <returns>The encrypted data string.</returns>
    /// <exception cref="MasterKeyNotFoundException">
    ///     The exception thrown when the encryption manager has not been initialized
    ///     yet.
    /// </exception>
    public string Encrypt(string data)
    {
        if (_masterKey == null)
            throw new MasterKeyNotFoundException();

        return EncryptionUtils.Encrypt(data, _masterKey);
    }

    /// <summary>
    ///     Decrypts the given string data using the master key of the current user.
    /// </summary>
    /// <param name="encryptedData">The encrypted string data.</param>
    /// <returns>The decrypted string value.</returns>
    /// <exception cref="MasterKeyNotFoundException">
    ///     The exception thrown when the encryption manager has not been initialized
    ///     yet.
    /// </exception>
    public string Decrypt(string encryptedData)
    {
        if (_masterKey == null)
            throw new MasterKeyNotFoundException();

        return EncryptionUtils.Decrypt(encryptedData, _masterKey);
    }


    /// <summary>
    ///     Serializes to json and encrypts the given data using the master key of the current user.
    /// </summary>
    /// <param name="data">The data to encrypt.</param>
    /// <typeparam name="T">The type of the data to encrypt.</typeparam>
    /// <returns>The encrypted data string.</returns>
    /// <exception cref="MasterKeyNotFoundException">
    ///     The exception thrown when the encryption manager has not been initialized
    ///     yet.
    /// </exception>
    public string SerializeEncrypt<T>(T data)
    {
        if (_masterKey == null)
            throw new MasterKeyNotFoundException();

        var serializedData = JsonSerializer.Serialize(data);

        return EncryptionUtils.Encrypt(serializedData, _masterKey);
    }

    /// <summary>
    ///     Decrypts the given data and deserializes it to the given type.
    /// </summary>
    /// <param name="encryptedData">The encrypted data as a string.</param>
    /// <typeparam name="T">The type of the object to deserialize to.</typeparam>
    /// <returns>The decrypted and deserialized object.</returns>
    /// <exception cref="MasterKeyNotFoundException">
    ///     The exception thrown when the encryption manager has not been initialized
    ///     yet.
    /// </exception>
    public T? DeserializeDecrypt<T>(string encryptedData)
    {
        if (_masterKey == null)
            throw new MasterKeyNotFoundException();

        var serializedData = EncryptionUtils.Decrypt(encryptedData, _masterKey);

        return JsonSerializer.Deserialize<T>(serializedData);
    }
}