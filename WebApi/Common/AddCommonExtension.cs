using System.Reflection;
using FluentValidation;
using MediatR;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi.Models;
using Quartz;
using Sieve.Services;
using WebApi.Common.Behaviours;
using WebApi.Common.Extensions;
using WebApi.Common.Services;
using WebApi.Core.Auth;

namespace WebApi.Common;

public static class AddCommonExtension
{
    /// <summary>
    ///     Add services related to the base functionality of the application (e.g. MediatR, FluentValidation, Swagger, etc.).
    ///     The services added here are likely to be used in every other part of the application either directly or indirectly.
    ///     It's import to only add only add services here that can't be categorized as part of a specific feature.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The app configuration.</param>
    public static void AddCommon(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.AddMemoryCache();

        //Commands
        services.AddConsoleCommands();

        //Lifetime actions
        services.AddLifetimeActions();

        // Fluent Validation
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.TryAddTransient<IValidatorFactory,
            ServiceProviderValidatorFactory>(); // Required for Swagger docs based on fluent validation

        //Swagger
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Blueboard", Version = "v4" });
            c.EnableAnnotations();
            c.AddAuthOperationFilters();
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
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
        });
        services.AddFluentValidationRulesToSwagger();

        // Filtering
        services.AddScoped<SieveProcessor, ApplicationSieveProcessor>();

        // MediatR
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));

        // Scheduler
        services.AddQuartz(q => { q.UseMicrosoftDependencyInjectionJobFactory(); });
        services.AddQuartzServer(options => { options.WaitForJobsToComplete = true; });

        // FluentEmail
        services
            .AddFluentEmail(configuration.GetValue<string>("Email:From"))
            .AddSmtpSender(configuration.GetValue<string>("Email:Host"),
                configuration.GetValue<int>("Email:Port"), configuration.GetValue<string>("Email:Username"),
                configuration.GetValue<string>("Email:Password"))
            .AddRazorRenderer();
    }
}