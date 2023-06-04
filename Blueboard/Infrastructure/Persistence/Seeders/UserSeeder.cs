using Blueboard.Core.Auth;
using Blueboard.Core.Auth.Services;
using Blueboard.Infrastructure.Persistence.Entities;
using Bogus;
using Helpers.Cryptography.Implementations;
using Helpers.Cryptography.Services;
using Microsoft.EntityFrameworkCore;

namespace Blueboard.Infrastructure.Persistence.Seeders;

public class UserSeeder
{
    private const int UserCount = 10;
    private const string ResetKeyPassword = "resetKeyPassword";
    private static readonly Faker Faker = new();

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
        _resetService.SetResetKeyPassword(ResetKeyPassword);

        var userGroup = new UserGroup { Id = AuthConstants.DefaultUserGroupID };
        _context.UserGroups.Entry(userGroup).State = EntityState.Unchanged;

        var users = new List<User>();
        for (var i = 0; i < UserCount; i++)
        {
            var user = CreateUser(string.Join(".", Faker.Lorem.Words(2)) + "@lovassy.edu.hu", Faker.Name.FullName(),
                "password", Faker.Random.ULong(10000000000, 99999999999).ToString(), verified);

            user.UserGroups = new List<UserGroup> { userGroup };
            users.Add(user);
        }

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