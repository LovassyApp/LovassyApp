using Blueboard.Core.Auth.Permissions;
using Blueboard.Core.Auth.Policies.EmailConfirmed;
using Blueboard.Core.Auth.Policies.Permissions;
using Blueboard.Features.ImageVotings.Queries;
using Helpers.WebApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using Sieve.Models;

namespace Blueboard.Features.ImageVotings;

[Authorize]
[EmailVerified]
[FeatureGate("ImageVotings")]
public class ImageVotingChoicesController : ApiControllerBase
{
    [HttpGet]
    [Permissions(typeof(ImageVotingsPermissions.IndexImageVotingChoices),
        typeof(ImageVotingsPermissions.IndexActiveImageVotingChoices))]
    [EndpointSummary("Get a list of all image voting choices")]
    public async Task<ActionResult<IEnumerable<IndexImageVotingChoices.Response>>> Index(
        [FromQuery] SieveModel sieveModel)
    {
        var response = await Mediator.Send(new IndexImageVotingChoices.Query
        {
            SieveModel = sieveModel
        });

        return Ok(response);
    }

    [HttpGet("{id}")]
    [Permissions(typeof(ImageVotingsPermissions.ViewImageVotingChoice),
        typeof(ImageVotingsPermissions.ViewActiveImageVotingChoice))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [EndpointSummary("Get information about an image voting choice")]
    public async Task<ActionResult<ViewImageVotingChoice.Response>> View([FromRoute] int id)
    {
        var response = await Mediator.Send(new ViewImageVotingChoice.Query
        {
            Id = id
        });

        return Ok(response);
    }
}