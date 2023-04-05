using Microsoft.OpenApi.Models;
using WebApi.Contexts.Import;
using WebApi.Contexts.Status;
using WebApi.Helpers.Cryptography;
using WebApi.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMemoryCache();

builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddCryptographyServices(builder.Configuration);

builder.Services.AddStatusContext(builder.Configuration);
builder.Services.AddImportContext();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "Blueboard", Version = "v4" }); });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();