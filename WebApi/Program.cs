using Hangfire;
using Microsoft.OpenApi.Models;
using WebApi.Contexts.Import;
using WebApi.Contexts.Status;
using WebApi.Contexts.Users;
using WebApi.Core.Auth;
using WebApi.Core.Cryptography;
using WebApi.Core.Scheduler;
using WebApi.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMemoryCache();

builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddCryptographyServices(builder.Configuration);
builder.Services.AddAuthServices(builder.Configuration);
builder.Services.AddSchedulerServices(builder.Configuration);

builder.Services.AddStatusContext(builder.Configuration);
builder.Services.AddImportContext();
builder.Services.AddUsersContext();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Blueboard", Version = "v4" });
    c.EnableAnnotations();
    c.AddAuthOperationFilters();

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