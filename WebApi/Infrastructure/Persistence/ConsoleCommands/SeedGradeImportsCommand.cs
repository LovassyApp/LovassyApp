using WebApi.Common.Models;
using WebApi.Common.Utils;
using WebApi.Infrastructure.Persistence.Seeders;

namespace WebApi.Infrastructure.Persistence.ConsoleCommands;

public class SeedGradeImportsCommand : IConsoleCommand
{
    private readonly IHostEnvironment _environment;
    private readonly GradeImportSeeder _gradeImportSeeder;

    public SeedGradeImportsCommand(IServiceProvider serviceProvider, IHostEnvironment environment)
    {
        var scope = serviceProvider.CreateScope();
        _gradeImportSeeder = scope.ServiceProvider.GetRequiredService<GradeImportSeeder>();
        _environment = environment;
    }

    public string Name { get; } = "seed:grade-imports";

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
            _gradeImportSeeder.RunAsync().Wait();
            ConsoleUtils.Success("Successfully seeded grade imports!");
        }
        else
        {
            ConsoleUtils.Error("Aborted seeding grade imports!");
        }
    }
}