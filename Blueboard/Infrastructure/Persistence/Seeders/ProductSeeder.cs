using Blueboard.Infrastructure.Persistence.Entities;
using Blueboard.Infrastructure.Persistence.Entities.Owned;
using Bogus;
using Microsoft.EntityFrameworkCore;

namespace Blueboard.Infrastructure.Persistence.Seeders;

public class ProductSeeder
{
    private const int ProductCount = 10;
    private static readonly Faker Faker = new();

    private readonly ApplicationDbContext _context;

    public ProductSeeder(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task RunAsync()
    {
        var qrCodes = await _context.QRCodes.ToListAsync();

        var products = new List<Product>();
        for (var i = 0; i < ProductCount; i++)
        {
            var product = CreateProduct(qrCodes);
            products.Add(product);
        }

        await _context.Products.AddRangeAsync(products);
        await _context.SaveChangesAsync();
    }

    private Product CreateProduct(List<QRCode> qrCodes)
    {
        var product = new Product
        {
            Name = Faker.Commerce.ProductName(),
            Description = Faker.Commerce.ProductDescription(),
            RichTextContent = Faker.Lorem.Paragraphs(),
            Visible = Faker.Random.Int(1, 3) != 1,
            QRCodeActivated = false,
            Price = Faker.Random.Int(1, 5),
            Quantity = Faker.Random.Bool() ? Faker.Random.Int(25, 50) : Faker.Random.Int(1, 10),
            Inputs = new List<ProductInput>(),
            NotifiedEmails = new string[] { },
            ThumbnailUrl = Faker.Image.LoremFlickrUrl()
        };

        //Notified emails
        if (Faker.Random.Bool())
        {
            var emails = new List<string>();
            for (var i = 0; i < Faker.Random.Int(1, 5); ++i) emails.Add(Faker.Internet.Email());
            product.NotifiedEmails = emails.ToArray();
        }

        //Has inputs
        if (Faker.Random.Bool())
        {
            var inputs = new List<ProductInput>();

            for (var i = 0; i < Faker.Random.Int(1, 5); ++i)
            {
                var input = new ProductInput
                {
                    Key = Faker.Lorem.Slug(2),
                    Label = string.Join(" ", Faker.Lorem.Words(2)),
                    Type = Faker.Random.Enum<ProductInputType>()
                };
                inputs.Add(input);
            }

            product.Inputs = inputs;
        }

        //QRCode activated
        if (qrCodes.Count > 0 && Faker.Random.Bool())
        {
            product.QRCodeActivated = true;
            product.QRCodes = new List<QRCode>();

            for (var i = 0; i < Faker.Random.Int(1, 5); i++)
            {
                var qrCode = qrCodes[Faker.Random.Int(0, qrCodes.Count - 1)];
                product.QRCodes.Add(qrCode);
            }
        }

        return product;
    }
}