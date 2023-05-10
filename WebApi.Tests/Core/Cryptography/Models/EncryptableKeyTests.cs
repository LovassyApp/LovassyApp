using FluentAssertions;
using NUnit.Framework;
using WebApi.Core.Cryptography.Exceptions;
using WebApi.Core.Cryptography.Models;

namespace WebApi.Tests.Core.Cryptography.Models;

public class EncryptableKeyTests
{
    [Test]
    public void ShouldLockAndUnlock()
    {
        var encryptableKey = new EncryptableKey();

        Assert.Throws<EncryptableKeyUnlockedException>(() => encryptableKey.Unlock("password", "salt"));

        encryptableKey.Lock("password", "salt");

        Assert.Throws<EncryptableKeyLockedException>(() => encryptableKey.Lock("password", "salt"));
        Assert.Throws<EncryptableKeyLockedException>(() => encryptableKey.GetKey());

        encryptableKey.Unlock("password", "salt");

        var key = encryptableKey.GetKey();

        key.Should().NotBeNull();
    }
}