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

        var serviceProvider = services.BuildServiceProvider();
        var encryptionManager = serviceProvider.GetRequiredService<EncryptionManager>();

        encryptionManager.Init(HashingUtils.GenerateBasicKey("password", "salt"));

        _hashManager = serviceProvider.GetRequiredService<HashManager>();
        _hashManager.Init(new User
        {
            HasherSaltEncrypted = encryptionManager.Encrypt(HashingUtils.GenerateSalt())
        });
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