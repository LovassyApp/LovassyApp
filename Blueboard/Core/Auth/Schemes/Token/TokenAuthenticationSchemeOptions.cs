using Microsoft.AspNetCore.Authentication;

namespace Blueboard.Core.Auth.Schemes.Token;

public class TokenAuthenticationSchemeOptions : AuthenticationSchemeOptions
{
    public string? HubsBasePath { get; set; }
}