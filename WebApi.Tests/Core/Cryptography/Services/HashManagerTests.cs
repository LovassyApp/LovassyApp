using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using WebApi.Core.Auth.Services;
using WebApi.Core.Cryptography.Services;
using WebApi.Core.Cryptography.Utils;
using WebApi.Infrastructure.Persistence;
using WebApi.Infrastructure.Persistence.Entities;

namespace WebApi.Tests.Core.Cryptography.Services;

public class HashManagerTests
{
    private HashManager _hashManager;

    [SetUp]
    public async Task SetUp()
    {
        var services = new ServiceCollection();

        services.AddMemoryCache();
        services.AddDbContext<ApplicationDbContext>();
        services.AddSingleton<SessionManager>();
        services.AddSingleton<EncryptionManager>();
        services.AddSingleton<HashManager>();
        services.AddSingleton<UserAccessor>();

        var serviceProvider = services.BuildServiceProvider();
        var encryptionManager = serviceProvider.GetRequiredService<EncryptionManager>();
        var userAccessor = serviceProvider.GetRequiredService<UserAccessor>();

        encryptionManager.SetMasterKeyTemporarily(HashingUtils.GenerateBasicKey("password", "salt"));
        userAccessor.User = new User
        {
            HasherSaltEncrypted = encryptionManager.Encrypt(HashingUtils.GenerateSalt())
        };

        _hashManager = serviceProvider.GetRequiredService<HashManager>();
    }

    [TestCase("id1")]
    [TestCase("id2")]
    [TestCase("id3")]
    public void ShouldHashAndVerifyId(string id)
    {
        var hashedId = _hashManager.HashWithHasherSalt(id);

        hashedId.Should().NotBeNull();
    }
}