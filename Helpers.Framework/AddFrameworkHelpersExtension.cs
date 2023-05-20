using System.Reflection;
using FluentValidation;
using Helpers.Framework.Behaviours;
using Helpers.Framework.Interfaces;
using Helpers.Framework.Services;
using MediatR;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi.Models;
using Quartz;
using Sieve.Services;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Helpers.Framework;

public static class AddFrameworkHelpersExtension
{
    /// <summary>
    ///     Adds all framework helpers to the service collection.
    /// </summary>
    /// <param name="services"></param>
    public static void AddFrameworkHelpers(this IServiceCollection services, IConfiguration configuration,
        Assembly assembly)
    {
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
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.TryAddTransient<IValidatorFactory,
            ServiceProviderValidatorFactory>(); // Required for Swagger docs based on fluent validation

        //Swagger
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Blueboard", Version = "v4" });

            var operationFilterTypes = assembly.GetTypes()
                .Where(t => t.IsAssignableTo(typeof(IOperationFilter)) &&
                            t is { IsInterface: false, IsAbstract: false });

            c.AddOperationFilters(assembly);

            c.CustomSchemaIds(type => type.ToString().Replace("WebApi.Features.", string.Empty)
                .Replace("Microsoft.AspNetCore.Mvc", string.Empty).Replace("+", string.Empty)
                .Replace(".", string.Empty).Replace("Commands", string.Empty).Replace("Queries", string.Empty));

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "Token",
                Scheme = "Bearer"
            });
            c.AddSecurityDefinition("ImportKey", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid import key",
                Name = "X-Authorization",
                Type = SecuritySchemeType.ApiKey
            });
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
    }
}