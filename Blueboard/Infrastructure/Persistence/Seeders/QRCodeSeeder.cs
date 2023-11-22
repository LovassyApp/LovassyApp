using Blueboard.Infrastructure.Persistence.Entities;
using Bogus;
using Org.BouncyCastle.Security;

namespace Blueboard.Infrastructure.Persistence.Seeders;

public class QRCodeSeeder(ApplicationDbContext context)
{
    private const int QRCodeCount = 10;
    private static readonly Faker Faker = new();

    public async Task RunAsync()
    {
        var qrcodes = new List<QRCode>();
        for (var i = 0; i < QRCodeCount; i++)
        {
            var qrcode = CreateQRCode();
            qrcodes.Add(qrcode);
        }

        await context.QRCodes.AddRangeAsync(qrcodes);
        await context.SaveChangesAsync();
    }

    private QRCode CreateQRCode()
    {
        var secureRandom = new SecureRandom();
        var secretBytes = new byte[24];
        secureRandom.NextBytes(secretBytes);

        var qrcode = new QRCode
        {
            Name = string.Join(" ", Faker.Lorem.Words()),
            Email = Faker.Internet.Email(),
            Secret = Convert.ToBase64String(secretBytes)
        };

        return qrcode;
    }
}