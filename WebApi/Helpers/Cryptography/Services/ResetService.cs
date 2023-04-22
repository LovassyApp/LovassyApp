using WebApi.Helpers.Cryptography.Exceptions;
using WebApi.Helpers.Cryptography.Utils;

namespace WebApi.Helpers.Cryptography.Services;

public class ResetService
{
    private readonly EncryptionService _encryptionService;

    private string? _resetKeyPassword;

    public ResetService(EncryptionService encryptionService)
    {
        _encryptionService = encryptionService;
    }

    private static string ResetPasswordSalt => "ResetPasswordSalt";

    public string EncryptMasterKey(string masterKey)
    {
        return _encryptionService.Encrypt(masterKey,
            HashingUtils.GenerateBasicKey(_resetKeyPassword ?? throw new ResetKeyPasswordMissingException(),
                ResetPasswordSalt));
    }

    public string DecryptMasterKey(string encryptedMasterKey)
    {
        return _encryptionService.Decrypt(encryptedMasterKey,
            HashingUtils.GenerateBasicKey(_resetKeyPassword ?? throw new ResetKeyPasswordMissingException(),
                ResetPasswordSalt));
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