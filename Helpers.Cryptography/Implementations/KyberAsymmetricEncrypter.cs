using Helpers.Cryptography.Interfaces;
using Helpers.Cryptography.Utils;
using Org.BouncyCastle.Pqc.Crypto.Crystals.Kyber;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;

namespace Helpers.Cryptography.Implementations;

/// <summary>
///     The implementation of <see cref="IAsymmetricEncrypter" /> using the Kyber post quantum encryption algorithm.
/// </summary>
/// <remarks>Not thread safe!</remarks>
public class KyberAsymmetricEncrypter : IAsymmetricEncrypter
{
    private readonly SecureRandom _random;

    public KyberAsymmetricEncrypter(string publicKey)
    {
        _random = new SecureRandom();

        PublicKey = new InternalPublicKey(publicKey);
    }

    private InternalPublicKey PublicKey { get; }

    public string Encrypt(string message)
    {
        var kemGenerator = new KyberKemGenerator(_random);
        var encapsulatedSecret = kemGenerator.GenerateEncapsulated(PublicKey.Key);
        var encryptionKey = encapsulatedSecret.GetSecret();

        var salt = HashingUtils.GenerateSalt();
        var encryptedMessage =
            EncryptionUtils.Encrypt(message,
                HashingUtils.GenerateBasicKey(Convert.ToBase64String(encryptionKey), salt));

        return Convert.ToBase64String(encapsulatedSecret.GetEncapsulation()) + "|" + salt + "|" + encryptedMessage;
    }

    /// <summary>
    ///     The internal public key class. Necessary because we have have to parse the encoded public key.
    /// </summary>
    private sealed class InternalPublicKey
    {
        private static readonly int PolyBytes = 384;
        private static readonly int SymBytes = 32;

        public InternalPublicKey(string publicKey)
        {
            var publicKeyBytes = Convert.FromBase64String(publicKey);
            var parameters = KyberParameters.kyber768;

            IndCpaSecretKeyBytes = parameters.K * PolyBytes;

            var t = Arrays.CopyOfRange(publicKeyBytes, 0,
                IndCpaSecretKeyBytes);

            var rho = Arrays.CopyOfRange(publicKeyBytes, IndCpaSecretKeyBytes,
                +IndCpaSecretKeyBytes + 32);

            Key = new KyberPublicKeyParameters(parameters, t, rho);
        }

        private int IndCpaSecretKeyBytes { get; }

        public KyberPublicKeyParameters Key { get; }
    }
}