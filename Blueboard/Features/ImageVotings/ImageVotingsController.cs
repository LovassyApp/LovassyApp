using Blueboard.Core.Auth.Permissions;
using Blueboard.Core.Auth.Policies.EmailConfirmed;
using Blueboard.Core.Auth.Policies.Permissions;
using Blueboard.Features.ImageVotings.Commands;
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
public class ImageVotingsController : ApiControllerBase
{
    [HttpGet]
    [Permissions(typeof(ImageVotingsPermissions.IndexImageVotings),
        typeof(ImageVotingsPermissions.IndexActiveImageVotings))]
    [EndpointSummary("Get a list of image votings")]
    public async Task<ActionResult<IEnumerable<IndexImageVotings.Response>>> Index([FromQuery] SieveModel sieveModel)
    {
        var response = await Mediator.Send(new IndexImageVotings.Query
        {
            SieveModel = sieveModel
        });

        return Ok(response);
    }

    [HttpGet("{id}")]
    [Permissions(typeof(ImageVotingsPermissions.ViewImageVoting),
        typeof(ImageVotingsPermissions.ViewActiveImageVoting))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [EndpointSummary("Get information about an image voting")]
    public async Task<ActionResult<ViewImageVoting.Response>> View([FromRoute] int id)
    {
        var response = await Mediator.Send(new ViewImageVoting.Query
        {
            Id = id
        });

        return Ok(response);
    }

    [HttpGet("{id}/Results")]
    [Permissions(typeof(ImageVotingsPermissions.ViewImageVotingResults),
        typeof(ImageVotingsPermissions.ViewActiveImageVotingResults))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [EndpointSummary("Get results of an image voting")]
    public async Task<ActionResult<ViewImageVotingResults.Response>> ViewResults([FromRoute] int id)
    {
        var response = await Mediator.Send(new ViewImageVotingResults.Query
        {
            Id = id
        });

        return Ok(response);
    }

    [HttpPost]
    [Permissions(typeof(ImageVotingsPermissions.CreateImageVoting))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [EndpointSummary("Create a new image voting")]
    public async Task<ActionResult<CreateImageVoting.Response>> Create([FromBody] CreateImageVoting.RequestBody body)
    {
        var response = await Mediator.Send(new CreateImageVoting.Command
        {
            Body = body
        });

        return CreatedAtAction(nameof(View), new { id = response.Id }, response);
    }

    [HttpPatch("{id}")]
    [Permissions(typeof(ImageVotingsPermissions.UpdateImageVoting))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [EndpointSummary("Update an image voting")]
    public async Task<ActionResult> Update([FromRoute] int id,
        [FromBody] UpdateImageVoting.RequestBody body)
    {
        await Mediator.Send(new UpdateImageVoting.Command
        {
            Id = id,
            Body = body
        });

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Permissions(typeof(ImageVotingsPermissions.DeleteImageVoting))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [EndpointSummary("Delete an image voting")]
    public async Task<ActionResult> Delete([FromRoute] int id)
    {
        await Mediator.Send(new DeleteImageVoting.Command
        {
            Id = id
        });

        return NoContent();
    }
}