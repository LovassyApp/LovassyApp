using System.Reflection;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;
using Helpers.Cryptography;
using Helpers.Framework;
using Helpers.Framework.Extensions;
using Helpers.Framework.Filters;
using Microsoft.AspNetCore.HttpOverrides;
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

builder.Services.AddCors();
builder.Services.AddRateLimiter(o =>
{
    o.RejectionStatusCode = 429;
    o.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
        RateLimitPartition.GetTokenBucketLimiter(
            context.Connection.RemoteIpAddress.ToString(),
            partition => new TokenBucketRateLimiterOptions
            {
                AutoReplenishment = true,
                TokenLimit = 120,
                ReplenishmentPeriod = TimeSpan.FromMinutes(2),
                TokensPerPeriod = 60
            }));

    o.AddPolicy("Strict", context => RateLimitPartition.GetFixedWindowLimiter(
        context.Connection.RemoteIpAddress.ToString() + context.Request.Path,
        partition => new FixedWindowRateLimiterOptions
        {
            AutoReplenishment = true,
            PermitLimit = 10,
            Window = TimeSpan.FromSeconds(30)
        }));
});
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

app.UseHttpsRedirection();

app.UseRouting();
app.UseCors(o => o.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});
app.UseRateLimiter();
app.UseHttpMetrics();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapMetrics();

app.RunWithCommands(args);