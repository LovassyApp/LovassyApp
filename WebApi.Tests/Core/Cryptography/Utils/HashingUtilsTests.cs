using FluentAssertions;
using NUnit.Framework;
using WebApi.Core.Cryptography.Utils;

namespace WebApi.Tests.Core.Cryptography.Utils;

public class HashingUtilsTests
{
    [TestCase("testData1")]
    [TestCase("testData2")]
    [TestCase("some different data")]
    public void ShouldHashAndVerify(string data)
    {
        var hashedData = HashingUtils.Hash(data);

        HashingUtils.Verify(data, hashedData).Should().Be(true);
    }

    [TestCase("testData1", "salt1")]
    [TestCase("testData2", "salt2")]
    [TestCase("some different data", "some other salt")]
    public void ShouldHashWithSaltAndVerify(string data, string salt)
    {
        var hashedData = HashingUtils.HashWithSalt(data, salt);

        HashingUtils.VerifyWithSalt(data, salt, hashedData).Should().Be(true);
    }

    [TestCase("password1", "salt1")]
    [TestCase("password2", "salt2")]
    [TestCase("some very different password", "some other salt")]
    public void ShouldGenerateBasicKey(string password, string salt)
    {
        var key = HashingUtils.GenerateBasicKey(password, salt);

        key.Should().NotBeNullOrEmpty();
        Convert.FromBase64String(key).Length.Should().Be(32);
    }

    [Test]
    public void ShouldGenerateRandomSalt()
    {
        var salt = HashingUtils.GenerateSalt();

        salt.Should().NotBeNullOrEmpty();
        Convert.FromBase64String(salt).Length.Should().Be(16);
    }
}