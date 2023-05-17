using FluentAssertions;
using Helpers.Cryptography.Implementations;
using NUnit.Framework;

namespace Helpers.Cryptography.Tests.Core.Cryptography.Implementations;

public class KyberAsymmetricEncrypterTests
{
    [TestCase("some data")]
    [TestCase("other data")]
    [TestCase("some different other data")]
    public void ShouldEncryptAndDecrypt(string data)
    {
        var kyberKeypair = new KyberKeypair();
        var encrypter = new KyberAsymmetricEncrypter(kyberKeypair.PublicKey);

        var encrypted = encrypter.Encrypt(data);
        var decrypted = kyberKeypair.Decrypt(encrypted);

        decrypted.Should().BeEquivalentTo(data);
    }
}