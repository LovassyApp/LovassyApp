using Blueboard.Infrastructure.Persistence.Seeders;
using Helpers.WebApi.Interfaces;
using Helpers.WebApi.Utils;

namespace Blueboard.Infrastructure.Persistence.ConsoleCommands;

public class SeedProductsCommand : IConsoleCommand
{
    private readonly IHostEnvironment _environment;
    private readonly ProductSeeder _productSeeder;

    public SeedProductsCommand(IServiceProvider serviceProvider, IHostEnvironment environment)
    {
        var scope = serviceProvider.CreateScope();
        _productSeeder = scope.ServiceProvider.GetRequiredService<ProductSeeder>();
        _environment = environment;
    }

    public string Name => "seed:products";

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
            _productSeeder.RunAsync().Wait();
            ConsoleUtils.Success("Successfully seeded products!");
        }
        else
        {
            ConsoleUtils.Error("Aborted seeding products!");
        }
    }
}