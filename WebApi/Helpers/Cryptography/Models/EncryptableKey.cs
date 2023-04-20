using System.Security.Cryptography;
using WebApi.Helpers.Cryptography.Exceptions;
using WebApi.Helpers.Cryptography.Traits.Extensions;
using WebApi.Helpers.Cryptography.Traits.Interfaces;

namespace WebApi.Helpers.Cryptography.Models;

public class EncryptableKey : IEncryptableKey, IUsesEncryption, IUsesHashing
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

        _keyEncrypted = this.Encrypt(_key!, this.GenerateBasicKey(password, salt));
        return _keyEncrypted;
    }

    public string Unlock(string password, string salt)
    {
        if (_unlocked)
            throw new EncryptableKeyUnlockedException();

        _key = this.Decrypt(_keyEncrypted!, this.GenerateBasicKey(password, salt));
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
}