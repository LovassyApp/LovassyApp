using Org.BouncyCastle.Pqc.Crypto.Crystals.Kyber;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using WebApi.Helpers.Cryptography.Traits.Extensions;
using WebApi.Helpers.Cryptography.Traits.Interfaces;

namespace WebApi.Helpers.Cryptography.Models;

public class KyberKeypair : IKeypair, IUsesEncryption, IUsesHashing
{
    private readonly InternalKeypair _keypair;
    private readonly SecureRandom _random;

    public KyberKeypair(string? privateKey = null)
    {
        _random = new SecureRandom();

        if (privateKey == null)
        {
            var keyGenParameters = new KyberKeyGenerationParameters(_random, KyberParameters.kyber768);
            var kyberKeyPairGenerator = new KyberKeyPairGenerator();
            kyberKeyPairGenerator.Init(keyGenParameters);

            var keyPair = kyberKeyPairGenerator.GenerateKeyPair();
            _keypair = new InternalKeypair((KyberPrivateKeyParameters)keyPair.Private,
                (KyberPublicKeyParameters)keyPair.Public);
        }
        else
        {
            var privateKeyBytes = Convert.FromBase64String(privateKey);
            _keypair = new InternalKeypair(KyberParameters.kyber768, privateKeyBytes);
        }
    }

    public string PrivateKey => Convert.ToBase64String(_keypair.PrivateKey.GetEncoded());

    public string PublicKey => Convert.ToBase64String(_keypair.PublicKey.GetEncoded());

    public string Encrypt(string message)
    {
        var kemGenerator = new KyberKemGenerator(_random);
        var encapsulatedSecret = kemGenerator.GenerateEncapsulated(_keypair.PublicKey);
        var encryptionKey = encapsulatedSecret.GetSecret();

        var salt = this.GenerateSalt();
        var encryptedMessage =
            this.Encrypt(message, this.GenerateBasicKey(Convert.ToBase64String(encryptionKey), salt));

        return Convert.ToBase64String(encapsulatedSecret.GetEncapsulation()) + "|" + salt + "|" + encryptedMessage;
    }

    public string Decrypt(string encryptedMessage)
    {
        var encapsulation = encryptedMessage.Split('|')[0];
        var salt = encryptedMessage.Split('|')[1];
        var message = encryptedMessage.Split('|')[2];

        var kemExtractor = new KyberKemExtractor(_keypair.PrivateKey);
        var encryptionKey = kemExtractor.ExtractSecret(Convert.FromBase64String(encapsulation));

        return this.Decrypt(message, this.GenerateBasicKey(Convert.ToBase64String(encryptionKey), salt));
    }

    private sealed class InternalKeypair
    {
        public static readonly int PolyBytes = 384;
        public static readonly int SymBytes = 32;

        public InternalKeypair(KyberParameters parameters, byte[] keyEncoded)
        {
            IndCpaPublicKeyBytes = parameters.K * PolyBytes + SymBytes;
            IndCpaSecretKeyBytes = parameters.K * PolyBytes;

            var s = Arrays.CopyOfRange(keyEncoded, 0, IndCpaPublicKeyBytes - 32);
            var t = Arrays.CopyOfRange(keyEncoded, IndCpaPublicKeyBytes - 32,
                IndCpaPublicKeyBytes - 32 + IndCpaSecretKeyBytes);
            var rho = Arrays.CopyOfRange(keyEncoded, IndCpaPublicKeyBytes - 32 + IndCpaSecretKeyBytes,
                IndCpaPublicKeyBytes + IndCpaSecretKeyBytes);
            var hpk = Arrays.CopyOfRange(keyEncoded, IndCpaPublicKeyBytes + IndCpaSecretKeyBytes,
                IndCpaPublicKeyBytes + IndCpaSecretKeyBytes + 32);
            var nonce = Arrays.CopyOfRange(keyEncoded, IndCpaPublicKeyBytes + IndCpaSecretKeyBytes + 32,
                IndCpaPublicKeyBytes + IndCpaSecretKeyBytes + 32 + SymBytes);

            PrivateKey = new KyberPrivateKeyParameters(parameters, s, hpk, nonce, t, rho);
            PublicKey = new KyberPublicKeyParameters(parameters, t, rho);
        }

        public InternalKeypair(KyberPrivateKeyParameters privateKey, KyberPublicKeyParameters publicKey)
        {
            PrivateKey = privateKey;
            PublicKey = publicKey;
        }

        private int IndCpaPublicKeyBytes { get; }
        private int IndCpaSecretKeyBytes { get; }

        public KyberPrivateKeyParameters PrivateKey { get; }
        public KyberPublicKeyParameters PublicKey { get; }
    }
}