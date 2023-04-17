namespace WebApi.Helpers.Cryptography.Services;

public interface IResetService
{
    public string EncryptMasterKey(string masterKey);
    public string DecryptMasterKey(string encryptedMasterKey);
    public void SetResetKeyPassword(string resetKeyPassword);
    public bool IsResetKeyPasswordSet();
}