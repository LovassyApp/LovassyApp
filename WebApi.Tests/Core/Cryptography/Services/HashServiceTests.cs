using FluentAssertions;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using WebApi.Core.Cryptography.Services;
using WebApi.Core.Cryptography.Services.Options;

namespace WebApi.Tests.Core.Cryptography.Services;

public class HashServiceTests
{
    private HashService _hashService;

    [SetUp]
    public void SetUp()
    {
        _hashService = new HashService(new OptionsWrapper<HashOptions>(new HashOptions()));
    }

    [TestCase("password1")]
    [TestCase("password2")]
    [TestCase("rE4liSt1cP4ssw0rd!")]
    public void ShouldHashAndVerifyPassword(string password)
    {
        var hashedPassword = _hashService.HashPassword(password);

        _hashService.VerifyPassword(password, hashedPassword).Should().Be(true);
    }
}