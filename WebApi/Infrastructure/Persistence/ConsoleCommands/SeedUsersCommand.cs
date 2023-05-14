using WebApi.Common.Models;
using WebApi.Common.Utils;
using WebApi.Infrastructure.Persistence.Seeders;

namespace WebApi.Infrastructure.Persistence.ConsoleCommands;

public class SeedUsersCommand : IConsoleCommand
{
    private readonly IHostEnvironment _environment;
    private readonly UserSeeder _userSeeder;

    public SeedUsersCommand(IServiceProvider serviceProvider, IHostEnvironment environment)
    {
        var scope = serviceProvider.CreateScope();
        _userSeeder = scope.ServiceProvider.GetRequiredService<UserSeeder>();
        _environment = environment;
    }

    public string Name { get; } = "seed:users";

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
            if (args is ["unverified", ..])
                _userSeeder.RunAsync(false).Wait();
            else
                _userSeeder.RunAsync().Wait();
            ConsoleUtils.Success("Successfully seeded users!");
        }
        else
        {
            ConsoleUtils.Error("Aborted seeding users!");
        }
    }
}