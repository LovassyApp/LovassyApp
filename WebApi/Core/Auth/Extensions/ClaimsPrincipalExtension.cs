using System.Security.Claims;
using WebApi.Core.Auth.Schemes.ImportKey;
using WebApi.Infrastructure.Persistence.Entities;

namespace WebApi.Core.Auth.Extensions;

public static class ClaimsPrincipalExtension
{
    /// <summary>
    ///     Gets the <see cref="User" /> id from the claims principal. When it's used in the context of an import key
    ///     authentication scheme,
    ///     it will return <see cref="ImportKey" /> id.
    ///     <seealso cref="ImportKeyAuthenticationSchemeHandler" />
    /// </summary>
    /// <param name="principal">The <see cref="ClaimsPrincipal" /> in which the id is stored.</param>
    /// <returns>Either the <see cref="User" /> or the <see cref="ImportKey" /> id.</returns>
    public static string? GetId(this ClaimsPrincipal principal)
    {
        return principal.FindFirstValue(ClaimTypes.NameIdentifier);
    }

    /// <summary>
    ///     Gets the name of the <see cref="User" /> from the claims principal. When it's used in the context of an import key
    ///     authentication
    ///     scheme, it will return the <see cref="ImportKey" /> name.
    ///     <seealso cref="ImportKeyAuthenticationSchemeHandler" />
    /// </summary>
    /// <param name="principal">The <see cref="ClaimsPrincipal" /> in which the name is stored.</param>
    /// <returns>Either the name of the <see cref="User" /> or the name of the <see cref="ImportKey" />.</returns>
    public static string? GetName(this ClaimsPrincipal principal)
    {
        return principal.FindFirstValue(ClaimTypes.Name);
    }

    /// <summary>
    ///     Gets the email of the <see cref="User" /> from the claims principal.
    /// </summary>
    /// <param name="principal">The <see cref="ClaimsPrincipal" /> in which the email is stored.</param>
    /// <returns>The email of the <see cref="User" />.</returns>
    public static string? GetEmail(this ClaimsPrincipal principal)
    {
        return principal.FindFirstValue(ClaimTypes.Email);
    }
}