using System.Reflection;
using System.Security.Claims;
using FluentValidation;
using Helpers.WebApi.Behaviours;
using Helpers.WebApi.Filters.Operation;
using Helpers.WebApi.Implementations;
using Helpers.WebApi.Interfaces;
using Helpers.WebApi.Services;
using Helpers.WebApi.Services.Options;
using MediatR;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.FeatureFilters;
using Microsoft.OpenApi.Models;
using Quartz;
using Sieve.Services;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Helpers.WebApi;

public static class AddFrameworkHelpersExtension
{
    /// <summary>
    ///     Adds all framework helpers to the service collection. Automatically discovers all commands, lifetime actions and
    ///     operation filters.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The app configuration.</param>
    /// <param name="assembly">The app assembly.</param>
    /// <param name="frameworkHelpersConfiguration">The configuration for the helper itself.</param>
    public static void AddFrameworkHelpers(this IServiceCollection services, IConfiguration configuration,
        Assembly assembly, FrameworkHelpersConfiguration? frameworkHelpersConfiguration = null)
    {
        frameworkHelpersConfiguration ??= new FrameworkHelpersConfiguration();

        services.AddHttpContextAccessor();
        services.AddMemoryCache();

        services.AddConsoleCommands(assembly);
        services.AddLifetimeActions(assembly);

        //MediatR
        services.AddMediatR(assembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));

        //Filtering
        services.AddScoped<SieveProcessor, ApplicationSieveProcessor>();

        // Fluent Validation
        services.AddValidatorsFromAssembly(assembly);
        services.TryAddTransient<IValidatorFactory,
            ServiceProviderValidatorFactory>(); // Required for Swagger docs based on fluent validation

        //Swagger
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1",
                new OpenApiInfo
                {
                    Title = frameworkHelpersConfiguration.ApiName, Version = frameworkHelpersConfiguration.ApiVersion
                });

            c.AddOperationFilters(assembly);

            c.CustomSchemaIds(frameworkHelpersConfiguration.SchemaIdResolver);

            foreach (var securityScheme in frameworkHelpersConfiguration.SecuritySchemes)
                c.AddSecurityDefinition(securityScheme.Key, securityScheme.Value);
        });
        services.AddFluentValidationRulesToSwagger();

        // FluentEmail
        services
            .AddFluentEmail(configuration.GetValue<string>("Email:From"))
            .AddSmtpSender(configuration.GetValue<string>("Email:Host"),
                configuration.GetValue<int>("Email:Port"), configuration.GetValue<string>("Email:Username"),
                configuration.GetValue<string>("Email:Password"))
            .AddRazorRenderer();

        // Scheduler
        services.AddQuartz(q => { q.UseMicrosoftDependencyInjectionJobFactory(); });
        services.AddQuartzServer(options => { options.WaitForJobsToComplete = true; });

        //Feature Flags
        services.AddFeatureManagement().AddFeatureFilter<TimeWindowFilter>().AddFeatureFilter<PercentageFilter>()
            .AddFeatureFilter<TargetingFilter>();
        services.AddSingleton<ITargetingContextAccessor, TargetingContextAccessor>();
        services.AddOptions<FeatureFlagOptions>();
        services.PostConfigureAll<FeatureFlagOptions>(o =>
        {
            o.FeatureGroupClaim = frameworkHelpersConfiguration.FeatureGroupClaim;
            o.FeatureUserClaim = frameworkHelpersConfiguration.FeatureUserClaim;
        });
    }

    private static void AddConsoleCommands(this IServiceCollection services, Assembly assembly)
    {
        var commandTypes = assembly.GetTypes()
            .Where(t => t.IsAssignableTo(typeof(IConsoleCommand)) && t is { IsInterface: false, IsAbstract: false });

        foreach (var commandType in commandTypes) services.AddTransient(typeof(IConsoleCommand), commandType);
    }

    private static void AddLifetimeActions(this IServiceCollection services, Assembly assembly)
    {
        var lifetimeActionTypes = assembly.GetTypes()
            .Where(t => t.IsAssignableTo(typeof(IStartupAction)) && t is { IsInterface: false, IsAbstract: false });

        foreach (var lifetimeActionType in lifetimeActionTypes)
            services.AddTransient(typeof(IStartupAction), lifetimeActionType);
    }

    private static void AddOperationFilters(this SwaggerGenOptions options, Assembly assembly)
    {
        var operationFilterTypes = assembly.GetTypes()
            .Where(t => t.IsAssignableTo(typeof(IOperationFilter)) && t is { IsInterface: false, IsAbstract: false });

        foreach (var operationFilterType in operationFilterTypes)
            options.OperationFilterDescriptors.Add(new FilterDescriptor
            {
                Type = operationFilterType,
                Arguments = new object[] { }
            });

        options.OperationFilter<FeatureGateOperationFilter>();
    }
}

public class FrameworkHelpersConfiguration
{
    public string ApiName { get; set; } = "API";
    public string ApiVersion { get; set; } = "v1";
    public Func<Type, string> SchemaIdResolver { get; set; } = type => type.ToString();
    public Dictionary<string, OpenApiSecurityScheme> SecuritySchemes { get; set; } = new();

    public string FeatureUserClaim { get; set; } = ClaimTypes.Email;
    public string FeatureGroupClaim { get; set; } = "UserGroup";
}