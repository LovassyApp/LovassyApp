using FluentAssertions;
using Helpers.Cryptography.Exceptions;
using Helpers.Cryptography.Implementations;
using NUnit.Framework;

namespace Helpers.Cryptography.Tests.Core.Cryptography.Implementations;

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