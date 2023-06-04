using Blueboard.Core.Auth.Interfaces;
using Blueboard.Core.Auth.Policies;
using Blueboard.Core.Auth.Policies.EmailConfirmed;
using Blueboard.Core.Auth.Policies.Permissions;
using Blueboard.Core.Auth.Schemes.ImportKey;
using Blueboard.Core.Auth.Schemes.ImportKey.ClaimsAdders;
using Blueboard.Core.Auth.Schemes.Token;
using Blueboard.Core.Auth.Schemes.Token.ClaimsAdders;
using Blueboard.Core.Auth.Services;
using Blueboard.Core.Auth.Services.Options;
using Blueboard.Infrastructure.Persistence.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using SessionOptions = Blueboard.Core.Auth.Services.Options.SessionOptions;

namespace Blueboard.Core.Auth;

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
        services.Configure<PermissionsOptions>(configuration.GetSection("Permissions"));
        services.Configure<EncryptionManagerOptions>(configuration.GetSection("Cryptography"));
        services.Configure<HashManagerOptions>(configuration.GetSection("Cryptography"));

        services.AddScoped<UserAccessor>();

        services.AddScoped<SessionManager>();
        services.AddScoped<EncryptionManager>();
        services.AddScoped<HashManager>();
        services.AddScoped<PermissionManager>();

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
        services.AddSingleton<IAuthorizationHandler, PermissionsHandler>();

        services.AddScoped<IClaimsAdder<ImportKey>, ImportKeyBaseClaimsAdder>();
        services.AddScoped<IClaimsAdder<User>, TokenBaseClaimsAdder>();
        services.AddScoped<IClaimsAdder<User>, TokenEmailConfirmedClaimsAdder>();
        services.AddScoped<IClaimsAdder<User>, TokenPermissionsClaimsAdder>();
        services.AddScoped<IClaimsAdder<User>, TokenFeatureGroupsClaimsAdder>();
    }
}