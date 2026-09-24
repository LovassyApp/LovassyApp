using Blueboard.Core.Auth.Permissions;
using Blueboard.Core.Auth.Policies.EmailConfirmed;
using Blueboard.Core.Auth.Policies.Permissions;
using Blueboard.Features.Increment.Commands;
using Helpers.WebApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;

namespace Blueboard.Features.Increment;

[Authorize]
[EmailVerified]
[FeatureGate("Increment")]
public class IncrementController : ApiControllerBase
{
    [HttpPost]
    [Permissions(typeof(IncrementPermissions.IncrementYear))]
    [EndpointSummary("Delete all users' grades, lolos, and owned saves to increment the academic year")]
    public async Task<ActionResult> Increment([FromBody] IncrementYear.RequestBody body)
    {
        await Mediator.Send(new IncrementYear.Command
        {
            Body = body
        });

        return NoContent();
    }
}