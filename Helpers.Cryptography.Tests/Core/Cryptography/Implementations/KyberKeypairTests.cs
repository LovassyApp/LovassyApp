using FluentAssertions;
using Helpers.Cryptography.Implementations;
using NUnit.Framework;

namespace Helpers.Cryptography.Tests.Core.Cryptography.Implementations;

public class KyberKeypairTests
{
    [TestCase("some data")]
    [TestCase("other data")]
    [TestCase("some other data")]
    public void ShouldEncryptAndDecrypt(string data)
    {
        var keypair = new KyberKeypair();
        var encrypted = keypair.Encrypt(data);
        var decrypted = keypair.Decrypt(encrypted);

        decrypted.Should().BeEquivalentTo(data);
    }

    [Test]
    public void ShouldDecode()
    {
        var keypair = new KyberKeypair();

        var encrypted = keypair.Encrypt("some data");
        var encodedPrivateKey = keypair.PrivateKey;

        var decodedKeypair = new KyberKeypair(encodedPrivateKey);
        var decrypted = decodedKeypair.Decrypt(encrypted);

        decrypted.Should().BeEquivalentTo("some data");
    }
}