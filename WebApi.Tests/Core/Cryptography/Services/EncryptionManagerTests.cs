using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using WebApi.Core.Auth.Services;
using WebApi.Core.Cryptography.Exceptions;
using WebApi.Core.Cryptography.Services;
using WebApi.Core.Cryptography.Utils;
using WebApi.Infrastructure.Persistence;

namespace WebApi.Tests.Core.Cryptography.Services;

public class EncryptionManagerTests
{
    private EncryptionManager _encryptionManager;

    [SetUp]
    public async Task SetUp()
    {
        var services = new ServiceCollection();

        services.AddMemoryCache();
        services.AddDbContext<ApplicationDbContext>();
        services.AddSingleton<SessionManager>();
        services.AddSingleton<EncryptionManager>();

        var serviceProvider = services.BuildServiceProvider();
        _encryptionManager = serviceProvider.GetRequiredService<EncryptionManager>();
    }

    [TestCase("testData1")]
    [TestCase("testData2")]
    [TestCase("other data")]
    public void ShouldEncryptAndDecryptWithMasterKey(string data)
    {
        _encryptionManager.MasterKey.Should().BeNull();

        Assert.Throws<MasterKeyNotFoundException>(() => _encryptionManager.Encrypt(data));

        _encryptionManager.Init(HashingUtils.GenerateBasicKey("password", "salt"));

        var encrypted = _encryptionManager.Encrypt(data);
        var decrypted = _encryptionManager.Decrypt(encrypted);

        decrypted.Should().BeEquivalentTo(data);
    }
}