using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Swashbuckle.AspNetCore.SwaggerGen;
using WebApi.Core.Auth.Filters.Operation;
using WebApi.Core.Auth.Interfaces;
using WebApi.Core.Auth.Permissions;
using WebApi.Core.Auth.Policies;
using WebApi.Core.Auth.Policies.EmailConfirmed;
using WebApi.Core.Auth.Schemes.ImportKey;
using WebApi.Core.Auth.Schemes.ImportKey.ClaimsAdders;
using WebApi.Core.Auth.Schemes.Token;
using WebApi.Core.Auth.Schemes.Token.ClaimsAdders;
using WebApi.Core.Auth.Services;
using WebApi.Infrastructure.Persistence.Entities;
using SessionOptions = WebApi.Core.Auth.Services.Options.SessionOptions;

namespace WebApi.Core.Auth;

public static class AddServicesExtension
{
    /// <summary>
    ///     Adds all auth related services that are likely to be used either directly or indirectly in some features.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The app configuration.</param>
    public static void AddAuthServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SessionOptions>(configuration.GetSection("Session"));
        services.AddScoped<SessionManager>();
        services.AddScoped<UserAccessor>();
        services.AddScoped<EncryptionManager>();
        services.AddScoped<HashManager>();
        services.AddSingleton<ResetService>();

        services
            .AddAuthentication(AuthConstants
                .TokenScheme) //TODO: Investigate this: Whenever we use a different scheme, we still run the handler for the default scheme, but we only challenge the other scheme (rn this has basically no effect, but it might be a problem in the future)
            .AddScheme<TokenAuthenticationSchemeOptions, TokenAuthenticationSchemeHandler>(AuthConstants.TokenScheme,
                o => { }) //TODO: Add HubsBasePath once there are hubs
            .AddScheme<ImportKeyAuthenticationSchemeOptions, ImportKeyAuthenticationSchemeHandler>(
                AuthConstants.ImportKeyScheme, o => { });

        services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();

        services.AddTransient<IPolicyEvaluator, ChallengeUnauthenticatedPolicyEvaluator>();
        services.AddTransient<PolicyEvaluator>();

        services.AddSingleton<IAuthorizationHandler, EmailVerifiedHandler>();

        services.AddScoped<IClaimsAdder<ImportKey>, ImportKeyBaseClaimsAdder>();
        services.AddScoped<IClaimsAdder<User>, TokenBaseClaimsAdder>();

        PermissionLookup.LoadPermissions(Assembly
            .GetExecutingAssembly()); // It's a bit unusual to do this here, but it's the only place where we can be sure that it's loaded before the first request
    }

    public static void AddAuthOperationFilters(this SwaggerGenOptions options)
    {
        options.OperationFilter<RequireImportKeyOperationFilter>();
        options.OperationFilter<AuthorizeOperationFilter>();
    }
}