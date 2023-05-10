using FluentAssertions;
using NUnit.Framework;
using WebApi.Core.Cryptography.Models;

namespace WebApi.Tests.Core.Cryptography.Models;

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