using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using WebApi.Core.Cryptography.Exceptions;
using WebApi.Core.Cryptography.Services;
using WebApi.Core.Cryptography.Utils;

namespace WebApi.Tests.Core.Cryptography.Services;

public class ResetServiceTests
{
    private ResetService _resetService;

    [SetUp]
    public void SetUp()
    {
        var services = new ServiceCollection();
        services.AddSingleton<ResetService>();

        var serviceProvider = services.BuildServiceProvider();
        _resetService = serviceProvider.GetRequiredService<ResetService>();
    }

    [TestCase("pwd1", "master key")]
    [TestCase("pwd2", "master key")]
    [TestCase("long password, yeah very long", "master key")]
    public void ShouldEncryptAndDecryptWithResetKeyPassword(string password, string masterKey)
    {
        _resetService.IsResetKeyPasswordSet().Should().Be(false);
        Assert.Throws<ResetKeyPasswordMissingException>(() => _resetService.EncryptMasterKey(masterKey, "salt"));

        _resetService.SetResetKeyPassword(password);
        _resetService.IsResetKeyPasswordSet().Should().Be(true);

        var salt = HashingUtils.GenerateSalt();

        var encrypted = _resetService.EncryptMasterKey(masterKey, salt);
        var decrypted = _resetService.DecryptMasterKey(encrypted, salt);

        decrypted.Should().BeEquivalentTo(masterKey);
    }
}