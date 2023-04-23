using Microsoft.AspNetCore.Authentication;

namespace WebApi.Core.Auth.Schemes.Token;

public class TokenAuthenticationSchemeOptions : AuthenticationSchemeOptions
{
    public string? HubsBasePath { get; set; }
}