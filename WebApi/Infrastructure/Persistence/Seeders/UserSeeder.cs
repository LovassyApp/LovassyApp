using Bogus;
using Helpers.Cryptography.Implementations;
using Helpers.Cryptography.Services;
using WebApi.Core.Auth.Services;
using WebApi.Infrastructure.Persistence.Entities;

namespace WebApi.Infrastructure.Persistence.Seeders;

public class UserSeeder
{
    private static readonly Faker Faker = new();
    private static readonly int _userCount = 10;
    private static readonly string _resetKeyPassword = "resetKeyPassword";

    private readonly ApplicationDbContext _context;
    private readonly EncryptionManager _encryptionManager;
    private readonly HashService _hashService;
    private readonly ResetService _resetService;

    public UserSeeder(ApplicationDbContext context, EncryptionManager encryptionManager,
        HashService hashService, ResetService resetService)
    {
        _context = context;
        _encryptionManager = encryptionManager;
        _hashService = hashService;
        _resetService = resetService;
    }

    public async Task RunAsync(bool verified = true)
    {
        _resetService.SetResetKeyPassword(_resetKeyPassword);

        var users = new List<User>();
        for (var i = 0; i < _userCount; i++)
            users.Add(CreateUser(string.Join(".", Faker.Lorem.Words(2)) + "@lovassy.edu.hu", Faker.Name.FullName(),
                "password", Faker.Random.ULong(10000000000, 99999999999).ToString(), verified));
        await _context.Users.AddRangeAsync(users);
        await _context.SaveChangesAsync();

        _resetService.SetResetKeyPassword(null);
    }


    private User CreateUser(string email, string name, string password, string omCode, bool verified)
    {
        var masterKeySalt = _hashService.GenerateSalt();
        var masterKey = new EncryptableKey();
        _encryptionManager.SetMasterKeyTemporarily(masterKey.GetKey());

        var storedKey = masterKey.Lock(password, masterKeySalt);

        var keys = new KyberKeypair();

        var hasherSalt = _hashService.GenerateSalt();

        return new User
        {
            Email = email,
            EmailVerifiedAt = verified ? DateTime.UtcNow : null,
            Name = name,
            PasswordHashed = _hashService.HashPassword(password),
            PublicKey = keys.PublicKey,
            PrivateKeyEncrypted = _encryptionManager.Encrypt(keys.PrivateKey),
            MasterKeyEncrypted = storedKey,
            MasterKeySalt = masterKeySalt,
            ResetKeyEncrypted = _resetService.EncryptMasterKey(_encryptionManager.MasterKey!, masterKeySalt),
            HasherSaltEncrypted = _encryptionManager.Encrypt(hasherSalt),
            HasherSaltHashed = _hashService.Hash(hasherSalt),
            OmCodeEncrypted = _encryptionManager.Encrypt(omCode),
            OmCodeHashed = _hashService.Hash(omCode)
        };
    }
}