using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Blueboard.Core.Auth;
using Blueboard.Core.Backboard;
using Blueboard.Core.Lolo;
using Blueboard.Core.Realtime;
using Blueboard.Features;
using Blueboard.Infrastructure;
using Helpers.Cryptography;
using Helpers.Email;
using Helpers.WebApi;
using Helpers.WebApi.Extensions;
using Helpers.WebApi.Filters;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using Npgsql;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddWebApiHelpers(builder.Configuration, Assembly.GetExecutingAssembly(),
    new FrameworkHelpersConfiguration
    {
        ApiName = "Blueboard",
        ApiVersion = "4.1.0",
        SchemaIdResolver = type => type.ToString().Replace("Blueboard.Features.", string.Empty)
            .Replace("Microsoft.AspNetCore.Mvc", string.Empty).Replace("+", string.Empty)
            .Replace(".", string.Empty).Replace("Commands", string.Empty).Replace("Queries", string.Empty),
        Servers =
        {
            new OpenApiServer { Url = "https://app.lovassy.hu" }, new OpenApiServer { Url = "http://127.0.0.1:5279" }
        },
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
builder.Services.AddEmailHelpers(builder.Configuration);
builder.Services.AddCryptographyHelpers(builder.Configuration);

var dataSourceBuilder = new NpgsqlDataSourceBuilder(builder.Configuration.GetConnectionString("DefaultConnection"));
dataSourceBuilder.EnableDynamicJson();

var dataSource = dataSourceBuilder.Build();

builder.Services.AddInfrastructure(dataSource);

builder.Services.AddAuthServices(builder.Configuration);
builder.Services.AddBackboardServices(builder.Configuration);
builder.Services.AddLoloServices(builder.Configuration);

builder.Services.AddFeatures(builder.Configuration);

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { "hu" };
    options.SetDefaultCulture(supportedCultures[0])
        .AddSupportedCultures(supportedCultures)
        .AddSupportedUICultures(supportedCultures);
});

builder.Services.AddCors(o =>
    o.AddDefaultPolicy(p => p.SetIsOriginAllowed(_ => true).AllowAnyMethod().AllowAnyHeader().AllowCredentials()));

builder.Services.AddControllers(o =>
{
    o.Filters.Add(new ExceptionFilter());

    // A fairly unideal solution but regular localization is a hot mess
    o.ModelBindingMessageProvider.SetAttemptedValueIsInvalidAccessor((x, y) =>
        $"Az érték '{x}' érvénytelen a(z) '{y}' mezőben.");
    o.ModelBindingMessageProvider.SetMissingBindRequiredValueAccessor(
        x => $"Nem lett megadva érték a(z) '{x}' mezőhöz.");
    o.ModelBindingMessageProvider.SetMissingKeyOrValueAccessor(() => "Kötelező értéket megadni.");
    o.ModelBindingMessageProvider.SetMissingRequestBodyRequiredValueAccessor(() => "A kérvény 'body' nem lehet üres.");
    o.ModelBindingMessageProvider.SetNonPropertyAttemptedValueIsInvalidAccessor(x => $"Az érték '{x}' nem érvényes.");
    o.ModelBindingMessageProvider.SetNonPropertyUnknownValueIsInvalidAccessor(() => "A megadott érték érvénytelen.");
    o.ModelBindingMessageProvider.SetNonPropertyValueMustBeANumberAccessor(() => "Az érték csak szám lehet.");
    o.ModelBindingMessageProvider.SetUnknownValueIsInvalidAccessor(x =>
        $"A megadott érték érvénytelen a(z) '{x}' mezőben.");
    o.ModelBindingMessageProvider.SetValueIsInvalidAccessor(x => $"Az érték '{x}' érvénytelen.");
    o.ModelBindingMessageProvider.SetValueMustBeANumberAccessor(x => $"A mező '{x}' értéke csak szám lehet.");
    o.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(x => $"Az érték '{x}' érvénytelen.");

    o.ModelValidatorProviders.Clear(); // Disable automatic model validation, fluent validation is used instead
}).AddJsonOptions(o =>
{
    o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    o.JsonSerializerOptions.DictionaryKeyPolicy =
        JsonNamingPolicy
            .CamelCase; // Because the validation errors need to be camel cased, IMPORTANT: pay attention to this when returning dictionaries
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSignalR();

var app = builder.Build();

await app.RunStartupActions();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Resources/FileUploads")),
    RequestPath = new PathString("/Files")
});

app.UseRouting();
app.UseCors();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});
app.UseRateLimiter();
// app.UseHttpMetrics();

app.UseAuthentication();
app.UseAuthorization();

app.UseRequestLocalization();

app.MapControllers();
app.MapRealtimeHubs();

app.MapMetrics();

app.RunWithCommands(args);