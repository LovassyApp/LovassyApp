using System.Security.Cryptography;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using WebApi.Core.Auth.Models;
using WebApi.Core.Cryptography.Services;
using WebApi.Features.Auth.Models;

namespace WebApi.Tests.Core.Cryptography.Services;

public class EncryptionServiceTests
{
    private EncryptionService _encryptionService;

    [SetUp]
    public void SetUp()
    {
        var services = new ServiceCollection();
        services.AddDataProtection();
        services.AddSingleton<EncryptionService>();

        var serviceProvider = services.BuildServiceProvider();
        _encryptionService = serviceProvider.GetRequiredService<EncryptionService>();
    }

    [TestCase("testData1")]
    [TestCase("testData2")]
    [TestCase("other data")]
    public void ShouldProtectAndUnprotect(string data)
    {
        var protectedData = _encryptionService.Protect(data);
        var unprotectedData = _encryptionService.Unprotect(protectedData);

        unprotectedData.Should().BeEquivalentTo(data);
    }

    [TestCase("string data1")]
    [TestCase("string data2")]
    [TestCase("other string data")]
    public void ShouldTimedProtectAndUnprotect(string data)
    {
        var protectedData = _encryptionService.Protect(data, TimeSpan.FromDays(1));
        var unprotectedData = _encryptionService.Unprotect(protectedData, out _);

        unprotectedData.Should().BeEquivalentTo(data);

        protectedData = _encryptionService.Protect(data, TimeSpan.FromDays(-1));

        Assert.Throws<CryptographicException>(() => _encryptionService.Unprotect(protectedData, out _));
    }

    [Test]
    public void ShouldProtectAndUnprotectRefreshTokenContents()
    {
        var data = new RefreshTokenContents
        {
            Password = "some password",
            UserId = Guid.NewGuid()
        };

        var protectedData = _encryptionService.SerializeProtect(data, TimeSpan.FromDays(1));
        var unprotectedData = _encryptionService.DeserializeUnprotect<RefreshTokenContents>(protectedData, out _);

        unprotectedData.Should().BeEquivalentTo(data);

        protectedData = _encryptionService.SerializeProtect(data, TimeSpan.FromDays(-1));

        Assert.Throws<CryptographicException>(() =>
            _encryptionService.DeserializeUnprotect<RefreshTokenContents>(protectedData, out _));
    }
}