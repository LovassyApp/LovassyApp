using FluentAssertions;
using Helpers.Cryptography.Utils;
using NUnit.Framework;

namespace Helpers.Cryptography.Tests.Core.Cryptography.Utils;

public class EncryptionUtilsTests
{
    [TestCase("testData1")]
    [TestCase("testData2")]
    [TestCase("some other data")]
    public void ShouldEncryptAndDecrypt(string data)
    {
        var key = HashingUtils.GenerateBasicKey("password", "salt");

        var encrypted = EncryptionUtils.Encrypt(data, key);
        var decrypted = EncryptionUtils.Decrypt(encrypted, key);

        decrypted.Should().BeEquivalentTo(data);
    }
}