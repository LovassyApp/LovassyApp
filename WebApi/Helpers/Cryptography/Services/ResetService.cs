using WebApi.Helpers.Cryptography.Exceptions;

namespace WebApi.Helpers.Cryptography.Services;

public class ResetService : IResetService
{
    private readonly IEncryptionService _encryptionService;

    private string? _resetKeyPassword;

    public ResetService(IEncryptionService encryptionService)
    {
        _encryptionService = encryptionService;
    }

    public string EncryptMasterKey(string masterKey)
    {
        return _encryptionService.Encrypt(masterKey, _resetKeyPassword ?? throw new ResetKeyPasswordMissingException());
    }

    public string DecryptMasterKey(string encryptedMasterKey)
    {
        return _encryptionService.Decrypt(encryptedMasterKey,
            _resetKeyPassword ?? throw new ResetKeyPasswordMissingException());
    }

    public void SetResetKeyPassword(string resetKeyPassword)
    {
        _resetKeyPassword = resetKeyPassword;
    }

    public bool IsResetKeyPasswordSet()
    {
        return _resetKeyPassword != null;
    }
}