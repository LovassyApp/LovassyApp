using System.Reflection;
using FluentValidation;
using Hangfire;
using MediatR;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi.Models;
using WebApi.Common;
using WebApi.Common.Filters;
using WebApi.Core.Auth;
using WebApi.Core.Cryptography;
using WebApi.Features;
using WebApi.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMemoryCache();
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
builder.Services
    .TryAddTransient<IValidatorFactory,
        ServiceProviderValidatorFactory>(); // Required for Swagger docs based on fluent validation
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

builder.Services.AddCommon(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddCryptographyServices(builder.Configuration);
builder.Services.AddAuthServices(builder.Configuration);

builder.Services.AddFeatures(builder.Configuration);

builder.Services.AddControllers(o => o.Filters.Add(new ExceptionFilter()));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
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
builder.Services.AddFluentValidationRulesToSwagger();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHangfireDashboard();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();