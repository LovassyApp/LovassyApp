using System.Reflection;
using System.Text.Json.Serialization;
using Helpers.Cryptography;
using Helpers.Framework;
using Helpers.Framework.Extensions;
using Helpers.Framework.Filters;
using Microsoft.OpenApi.Models;
using Prometheus;
using WebApi.Core.Auth;
using WebApi.Core.Backboard;
using WebApi.Core.Lolo;
using WebApi.Features;
using WebApi.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddFrameworkHelpers(builder.Configuration, Assembly.GetExecutingAssembly(),
    new FrameworkHelpersConfiguration
    {
        ApiName = "Blueboard",
        ApiVersion = "v4",
        SchemaIdResolver = type => type.ToString().Replace("WebApi.Features.", string.Empty)
            .Replace("Microsoft.AspNetCore.Mvc", string.Empty).Replace("+", string.Empty)
            .Replace(".", string.Empty).Replace("Commands", string.Empty).Replace("Queries", string.Empty),
        SecuritySchemes = new Dictionary<string, OpenApiSecurityScheme>
        {
            [AuthConstants.TokenScheme] = new()
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "Token",
                Scheme = "Bearer"
            },
            [AuthConstants.ImportKeyScheme] = new()
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid import key",
                Name = "X-Authorization",
                Type = SecuritySchemeType.ApiKey
            }
        },
        FeatureGroupClaim = AuthConstants.FeatureGroupClaim,
        FeatureUserClaim = AuthConstants.FeatureUserClaim
    });
builder.Services.AddCryptographyHelpers(builder.Configuration);

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddAuthServices(builder.Configuration);
builder.Services.AddBackboardServices(builder.Configuration);
builder.Services.AddLoloServices(builder.Configuration);

builder.Services.AddFeatures(builder.Configuration);

builder.Services.AddControllers(o => o.Filters.Add(new ExceptionFilter())).AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
;
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

await app.RunStartupActions();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//TODO: Rate limiting!!!
app.UseHttpsRedirection();

app.UseRouting();
app.UseHttpMetrics();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapMetrics();

app.RunWithCommands(args);