using Bogus;
using Org.BouncyCastle.Security;
using WebApi.Infrastructure.Persistence.Entities;

namespace WebApi.Infrastructure.Persistence.Seeders;

public class QRCodeSeeder
{
    private const int QRCodeCount = 10;
    private static readonly Faker Faker = new();

    private readonly ApplicationDbContext _context;

    public QRCodeSeeder(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task RunAsync()
    {
        var qrcodes = new List<QRCode>();
        for (var i = 0; i < QRCodeCount; i++)
        {
            var qrcode = CreateQRCode();
            qrcodes.Add(qrcode);
        }

        await _context.QRCodes.AddRangeAsync(qrcodes);
        await _context.SaveChangesAsync();
    }

    private QRCode CreateQRCode()
    {
        var secureRandom = new SecureRandom();
        var secretBytes = new byte[24];
        secureRandom.NextBytes(secretBytes);

        var qrcode = new QRCode
        {
            Name = string.Join(" ", Faker.Lorem.Words()),
            Secret = Convert.ToBase64String(secretBytes)
        };

        return qrcode;
    }
}