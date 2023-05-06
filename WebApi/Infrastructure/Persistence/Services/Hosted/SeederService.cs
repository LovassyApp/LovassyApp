using WebApi.Infrastructure.Persistence.Seeders;

namespace WebApi.Infrastructure.Persistence.Services.Hosted;

public class SeederService : IHostedService
{
    private readonly IHostEnvironment _environment;
    private readonly GradeImportSeeder _gradeImportSeeder;

    public SeederService(IServiceProvider serviceProvider, IHostEnvironment environment)
    {
        var scope = serviceProvider.CreateScope();
        _gradeImportSeeder = scope.ServiceProvider.GetRequiredService<GradeImportSeeder>();
        _environment = environment;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        //TODO: Seed default warden group
        if (_environment.IsDevelopment())
        {
            // Uncomment if you want to seed the database with grade imports
            //await _gradeImportSeeder.RunAsync();
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
    }
}