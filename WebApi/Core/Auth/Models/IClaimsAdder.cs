using System.Security.Claims;

namespace WebApi.Core.Auth.Models;

public interface IClaimsAdder<in TUser>
{
    public Task AddClaimsAsync(List<Claim> claims, TUser user);
}