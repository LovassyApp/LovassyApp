using EFCoreSecondLevelCacheInterceptor;
using Microsoft.EntityFrameworkCore;
using WebApi.Infrastructure.Persistence;
using WebApi.Infrastructure.Persistence.Seeders;

namespace WebApi.Infrastructure;

public static class AddInfrastructureExtension
{
    /// <summary>
    ///     Adds all services related to infrastructure, aka interfacing with external systems (for example the database).
    ///     Services added here are likely to be used either directly or indirectly in every part of the application (but it's
    ///     not a must).
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The app configuration.</param>
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

        //Seeding
        services.AddScoped<GradeImportSeeder>();
        services.AddScoped<UserSeeder>();
    }
}