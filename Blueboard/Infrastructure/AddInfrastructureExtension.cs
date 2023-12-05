using Blueboard.Infrastructure.Files.Services;
using Blueboard.Infrastructure.Persistence;
using Blueboard.Infrastructure.Persistence.Seeders;
using EFCoreSecondLevelCacheInterceptor;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Blueboard.Infrastructure;

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
#pragma warning disable CS0618 // Type or member is obsolete
        NpgsqlConnection.GlobalTypeMapper.EnableDynamicJson();
#pragma warning restore CS0618 // Type or member is obsolete

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

        // Seeding
        services.AddScoped<GradeImportSeeder>();
        services.AddScoped<UserSeeder>();
        services.AddScoped<ProductSeeder>();
        services.AddScoped<QRCodeSeeder>();

        // Files
        services.AddScoped<FilesService>();
    }
}