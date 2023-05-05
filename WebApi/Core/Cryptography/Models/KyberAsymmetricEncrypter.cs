using Org.BouncyCastle.Pqc.Crypto.Crystals.Kyber;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using WebApi.Core.Cryptography.Utils;

namespace WebApi.Core.Cryptography.Models;

public class KyberAsymmetricEncrypter : IAsymmetricEncrypter
{
    private static readonly int PolyBytes = 384;
    private static readonly int SymBytes = 32;

    private readonly SecureRandom _random;

    public KyberAsymmetricEncrypter(string publicKey)
    {
        _random = new SecureRandom();

        var publicKeyBytes = Convert.FromBase64String(publicKey);
        var parameters = KyberParameters.kyber768;

        IndCpaSecretKeyBytes = parameters.K * PolyBytes;

        var t = Arrays.CopyOfRange(publicKeyBytes, 0,
            IndCpaSecretKeyBytes);

        var rho = Arrays.CopyOfRange(publicKeyBytes, IndCpaSecretKeyBytes,
            +IndCpaSecretKeyBytes + 32);

        PublicKey = new KyberPublicKeyParameters(parameters, t, rho);
    }

    private int IndCpaSecretKeyBytes { get; }
    private KyberPublicKeyParameters PublicKey { get; }

    public string Encrypt(string message)
    {
        var kemGenerator = new KyberKemGenerator(_random);
        var encapsulatedSecret = kemGenerator.GenerateEncapsulated(PublicKey);
        var encryptionKey = encapsulatedSecret.GetSecret();

        var salt = HashingUtils.GenerateSalt();
        var encryptedMessage =
            EncryptionUtils.Encrypt(message,
                HashingUtils.GenerateBasicKey(Convert.ToBase64String(encryptionKey), salt));

        return Convert.ToBase64String(encapsulatedSecret.GetEncapsulation()) + "|" + salt + "|" + encryptedMessage;
    }
}