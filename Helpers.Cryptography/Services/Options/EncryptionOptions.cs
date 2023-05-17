namespace Helpers.Cryptography.Services.Options;

public class EncryptionOptions
{
    public string DataProtectionPurpose { get; set; } = "Blueboard";
    public string MasterKeySessionKey { get; set; } = "master_key";
}