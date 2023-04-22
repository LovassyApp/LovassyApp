using System.Text.Json;
using Microsoft.Extensions.Options;
using WebApi.Helpers.Auth.Exceptions;
using WebApi.Helpers.Auth.Services;
using WebApi.Helpers.Cryptography.Exceptions;
using WebApi.Helpers.Cryptography.Services.Options;
using WebApi.Helpers.Cryptography.Utils;

namespace WebApi.Helpers.Cryptography.Services;

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
            _sessionManager.Set(_masterKeySessionKey, value);
        }
    }

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

            _masterKey = _sessionManager.Get(_masterKeySessionKey);

            if (_masterKey == null)
                throw new MasterKeyNotFoundException();
        }
    }

    public string Encrypt(string data)
    {
        if (_masterKey == null)
            throw new MasterKeyNotFoundException();

        return EncryptionUtils.Encrypt(data, _masterKey);
    }

    public string Decrypt(string encryptedData)
    {
        if (_masterKey == null)
            throw new MasterKeyNotFoundException();

        return EncryptionUtils.Decrypt(encryptedData, _masterKey);
    }

    public string SerializeEncrypt<T>(T data)
    {
        if (_masterKey == null)
            throw new MasterKeyNotFoundException();

        var serializedData = JsonSerializer.Serialize(data);

        return EncryptionUtils.Encrypt(serializedData, _masterKey);
    }

    public T? SerializeDecrypt<T>(string encryptedData)
    {
        if (_masterKey == null)
            throw new MasterKeyNotFoundException();

        var serializedData = EncryptionUtils.Decrypt(encryptedData, _masterKey);

        return JsonSerializer.Deserialize<T>(serializedData);
    }
}