using System.Reflection;
using FluentValidation;
using Hangfire;
using Hangfire.PostgreSql;
using MediatR;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi.Models;
using Sieve.Services;
using WebApi.Common.Behaviours;
using WebApi.Common.Extensions;
using WebApi.Common.Services;
using WebApi.Core.Auth;

namespace WebApi.Common;

public static class AddCommonExtension
{
    public static void AddCommon(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.AddMemoryCache();

        //Commands
        services.AddConsoleCommands();

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
        services.AddHangfire(c => c.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UsePostgreSqlStorage(configuration.GetConnectionString("HangfireConnection")));
        services.AddHangfireServer();
    }
}