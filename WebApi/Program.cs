using System.Reflection;
using System.Text.Json.Serialization;
using Helpers.Cryptography;
using Helpers.Framework;
using Helpers.Framework.Extensions;
using Helpers.Framework.Filters;
using Prometheus;
using WebApi.Core.Auth;
using WebApi.Core.Backboard;
using WebApi.Core.Lolo;
using WebApi.Features;
using WebApi.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddFrameworkHelpers(builder.Configuration, Assembly.GetExecutingAssembly());
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