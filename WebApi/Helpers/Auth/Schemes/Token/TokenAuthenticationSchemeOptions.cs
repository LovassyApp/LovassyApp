using Microsoft.AspNetCore.Authentication;

namespace WebApi.Helpers.Auth.Schemes.Token;

public class TokenAuthenticationSchemeOptions : AuthenticationSchemeOptions
{
    public string? HubsBasePath { get; set; }
}