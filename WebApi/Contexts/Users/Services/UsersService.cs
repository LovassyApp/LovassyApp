using WebApi.Contexts.Users.Models;
using WebApi.Helpers.Cryptography.Exceptions;
using WebApi.Helpers.Cryptography.Models;
using WebApi.Helpers.Cryptography.Services;
using WebApi.Persistence;
using WebApi.Persistence.Entities;

namespace WebApi.Contexts.Users.Services;

public class UsersService
{
    private readonly ApplicationDbContext _context;
    private readonly EncryptionManager _encryptionManager;
    private readonly HashService _hashService;
    private readonly ResetService _resetService;

    public UsersService(ApplicationDbContext context, HashService hashService, EncryptionManager encryptionManager,
        ResetService resetService)
    {
        _context = context;
        _hashService = hashService;
        _encryptionManager = encryptionManager;
        _resetService = resetService;
    }

    public async Task CreateUserAsync(CreateUserDto data)
    {
        if (!_resetService.IsResetKeyPasswordSet()) throw new ResetKeyPasswordMissingException();

        var masterKeySalt = _hashService.GenerateSalt();
        var masterKey = new EncryptableKey();
        _encryptionManager.Init(masterKey.GetKey());

        var storedKey = masterKey.Lock(data.Password, masterKeySalt);

        var keys = new KyberKeypair();

        var hasherSalt = _hashService.GenerateSalt();

        var user = new User
        {
            Email = data.Email,
            Name = data.Name,
            PasswordHashed = _hashService.HashPassword(data.Password),
            PublicKey = keys.PublicKey,
            PrivateKeyEncrypted = _encryptionManager.Encrypt(keys.PrivateKey),
            MasterKeyEncrypted = storedKey,
            MasterKeySalt = masterKeySalt,
            ResetKeyEncrypted = _resetService.EncryptMasterKey(_encryptionManager.MasterKey!),
            HasherSaltEncrypted = _encryptionManager.Encrypt(hasherSalt),
            HasherSaltHashed = _hashService.Hash(hasherSalt),
            OmCodeEncrypted = _encryptionManager.Encrypt(data.OmCode),
            OmCodeHashed = _hashService.Hash(data.OmCode)
        };

        await _context.Users.AddAsync(user);

        //TODO: Add user to default group

        await _context.SaveChangesAsync();
    }
}