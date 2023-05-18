using System.Security.Claims;

namespace Helpers.Framework.Interfaces;

/// <summary>
///     The interface for adding claims to a <see cref="ClaimsPrincipal" />. Used in authentication schemes. They have to
///     be registered manually.
/// </summary>
/// <typeparam name="TUser"></typeparam>
public interface IClaimsAdder<in TUser>
{
    public Task AddClaimsAsync(List<Claim> claims, TUser user);
}