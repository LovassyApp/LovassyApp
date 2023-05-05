using EFCoreSecondLevelCacheInterceptor;
using Microsoft.EntityFrameworkCore;
using WebApi.Infrastructure.Persistence;
using WebApi.Infrastructure.Persistence.Seeders;
using WebApi.Infrastructure.Persistence.Services.Hosted;

namespace WebApi.Infrastructure;

public static class AddInfrastructureExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Persistence
        services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
                .AddInterceptors(serviceProvider.GetRequiredService<SecondLevelCacheInterceptor>());
        });

        services.AddEFSecondLevelCache(options =>
            {
                options.UseMemoryCacheProvider().DisableLogging(true).UseCacheKeyPrefix("EF_");
                options.CacheAllQueries(CacheExpirationMode.Absolute, TimeSpan.FromDays(7));
            }
        );

        services.AddScoped<GradeImportSeeder>();
        services.AddHostedService<SeederService>();
    }
}