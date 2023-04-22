using Microsoft.AspNetCore.Authorization;
using WebApi.Helpers.Auth.Schemes.Token;
using WebApi.Helpers.Auth.Services;
using SessionOptions = WebApi.Helpers.Auth.Services.Options.SessionOptions;

namespace WebApi.Helpers.Auth;

public static class AddServicesExtension
{
    public static void AddAuthServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SessionOptions>(configuration.GetSection("Session"));
        services.AddScoped<SessionManager>();

        services.AddAuthentication()
            .AddScheme<TokenAuthenticationSchemeOptions, TokenAuthenticationSchemeHandler>(AuthConstants.TokenScheme,
                o => { });

        services.AddAuthorization(o =>
        {
            o.DefaultPolicy = new AuthorizationPolicyBuilder().AddAuthenticationSchemes(AuthConstants.TokenScheme)
                .RequireAuthenticatedUser().Build(); //TODO: Replace with the policy of Warden
        });
    }
}