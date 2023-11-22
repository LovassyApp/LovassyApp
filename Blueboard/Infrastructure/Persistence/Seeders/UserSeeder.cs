using Blueboard.Core.Auth;
using Blueboard.Core.Auth.Services;
using Blueboard.Infrastructure.Persistence.Entities;
using Bogus;
using Helpers.Cryptography.Implementations;
using Helpers.Cryptography.Services;
using Microsoft.EntityFrameworkCore;

namespace Blueboard.Infrastructure.Persistence.Seeders;

public class UserSeeder(ApplicationDbContext context, EncryptionManager encryptionManager,
    HashService hashService, ResetService resetService)
{
    private const int UserCount = 10;
    private const string ResetKeyPassword = "resetKeyPassword";
    private static readonly Faker Faker = new();

    public async Task RunAsync(bool verified = true)
    {
        resetService.SetResetKeyPassword(ResetKeyPassword);

        var userGroup = new UserGroup { Id = AuthConstants.DefaultUserGroupID };
        context.UserGroups.Entry(userGroup).State = EntityState.Unchanged;

        var users = new List<User>();
        for (var i = 0; i < UserCount; i++)
        {
            var user = CreateUser(string.Join(".", Faker.Lorem.Words(2)) + "@lovassy.edu.hu", Faker.Name.FullName(),
                "password", Faker.Random.ULong(10000000000, 99999999999).ToString(), verified);

            user.UserGroups = new List<UserGroup> { userGroup };
            users.Add(user);
        }

        await context.Users.AddRangeAsync(users);
        await context.SaveChangesAsync();

        resetService.SetResetKeyPassword(null);
    }


    private User CreateUser(string email, string name, string password, string omCode, bool verified)
    {
        var masterKeySalt = hashService.GenerateSalt();
        var masterKey = new EncryptableKey();
        encryptionManager.SetMasterKeyTemporarily(masterKey.GetKey());

        var storedKey = masterKey.Lock(password, masterKeySalt);

        var keys = new KyberKeypair();

        var hasherSalt = hashService.GenerateSalt();

        return new User
        {
            Email = email,
            EmailVerifiedAt = verified ? DateTime.UtcNow : null,
            Name = name,
            PasswordHashed = hashService.HashPassword(password),
            PublicKey = keys.PublicKey,
            PrivateKeyEncrypted = encryptionManager.Encrypt(keys.PrivateKey),
            MasterKeyEncrypted = storedKey,
            MasterKeySalt = masterKeySalt,
            ResetKeyEncrypted = resetService.EncryptMasterKey(encryptionManager.MasterKey!, masterKeySalt),
            HasherSaltEncrypted = encryptionManager.Encrypt(hasherSalt),
            HasherSaltHashed = hashService.Hash(hasherSalt),
            OmCodeEncrypted = encryptionManager.Encrypt(omCode),
            OmCodeHashed = hashService.Hash(omCode)
        };
    }
}