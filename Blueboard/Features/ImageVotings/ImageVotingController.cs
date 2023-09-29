using Blueboard.Core.Auth.Permissions;
using Blueboard.Core.Auth.Policies.EmailConfirmed;
using Blueboard.Core.Auth.Policies.Permissions;
using Blueboard.Features.ImageVotings.Commands;
using Blueboard.Features.ImageVotings.Queries;
using Helpers.WebApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blueboard.Features.ImageVotings;

[Authorize]
[EmailVerified]
public class ImageVotingController : ApiControllerBase
{
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

    [HttpPost]
    [Permissions(typeof(ImageVotingsPermissions.CreateImageVoting))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes
        .Status503ServiceUnavailable)] //TODO: Remove this when the incremental feature is implemented
    [EndpointSummary("Create a new image voting")]
    public async Task<ActionResult<CreateImageVoting.Response>> Create([FromBody] CreateImageVoting.RequestBody body)
    {
        var response = await Mediator.Send(new CreateImageVoting.Command
        {
            Body = body
        });

        return CreatedAtAction(nameof(View), new { id = response.Id }, response);
    }
}