using Helpers.Framework.Interfaces;
using Helpers.Framework.Utils;
using WebApi.Infrastructure.Persistence.Seeders;

namespace WebApi.Infrastructure.Persistence.ConsoleCommands;

public class SeedQRCodesCommand : IConsoleCommand
{
    private readonly IHostEnvironment _environment;
    private readonly QRCodeSeeder _qrCodeSeeder;

    public SeedQRCodesCommand(IServiceProvider serviceProvider, IHostEnvironment environment)
    {
        var scope = serviceProvider.CreateScope();
        _qrCodeSeeder = scope.ServiceProvider.GetRequiredService<QRCodeSeeder>();
        _environment = environment;
    }

    public string Name { get; } = "seed:qrcodes";

    public void Execute(string[] args)
    {
        var continueSeeding = true;

        if (!_environment.IsDevelopment())
        {
            continueSeeding = false;
            ConsoleUtils.Warning("You are not running in development mode. Are you sure you want to continue? (y/n)");
            var input = Console.ReadLine();
            if (input != null && input.ToLower() == "y") continueSeeding = true;
        }

        if (continueSeeding)
        {
            _qrCodeSeeder.RunAsync().Wait();
            ConsoleUtils.Success("Successfully seeded qrcodes!");
        }
        else
        {
            ConsoleUtils.Error("Aborted seeding qrcodes!");
        }
    }
}