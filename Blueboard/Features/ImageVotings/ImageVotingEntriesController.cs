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
public class ImageVotingEntriesController : ApiControllerBase
{
    [HttpGet("{id}")]
    [Permissions(typeof(ImageVotingsPermissions.ViewImageVotingEntry),
        typeof(ImageVotingsPermissions.ViewActiveImageVotingEntry))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [EndpointSummary("Get information about an image voting entry")]
    public async Task<ActionResult<ViewImageVotingEntry.Response>> View([FromRoute] int id)
    {
        var response = await Mediator.Send(new ViewImageVotingEntry.Query
        {
            Id = id
        });

        return Ok(response);
    }

    [HttpGet]
    [Permissions(typeof(ImageVotingsPermissions.IndexImageVotingEntries),
        typeof(ImageVotingsPermissions.IndexActiveImageVotingEntries))]
    [EndpointSummary("Get a list of all image voting entries")]
    public async Task<ActionResult<IEnumerable<IndexImageVotingEntries.Response>>> Index(
        [FromQuery] SieveModel sieveModel)
    {
        var response = await Mediator.Send(new IndexImageVotingEntries.Query
        {
            SieveModel = sieveModel
        });

        return Ok(response);
    }


    [HttpPost]
    [Permissions(typeof(ImageVotingsPermissions.ChooseImageVotingEntry),
        typeof(ImageVotingsPermissions.CreateActiveImageVotingEntry))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [EndpointSummary("Create an image voting entry")]
    public async Task<ActionResult<CreateImageVotingEntry.Response>> CreateImageVotingEntry(
        [FromBody] CreateImageVotingEntry.RequestBody body)
    {
        var response = await Mediator.Send(new CreateImageVotingEntry.Command
        {
            Body = body
        });

        return CreatedAtAction(nameof(View), new { id = response.Id }, response);
    }

    [HttpPatch("{id}")]
    [Permissions(typeof(ImageVotingsPermissions.UpdateImageVotingEntry),
        typeof(ImageVotingsPermissions.UpdateOwnImageVotingEntry))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [EndpointSummary("Update an image voting entry")]
    public async Task<ActionResult> UpdateImageVotingEntry([FromRoute] int id,
        [FromBody] UpdateImageVotingEntry.RequestBody body)
    {
        await Mediator.Send(new UpdateImageVotingEntry.Command
        {
            Id = id,
            Body = body
        });

        return NoContent();
    }


    [HttpDelete("{id}")]
    [Permissions(typeof(ImageVotingsPermissions.DeleteImageVotingEntry),
        typeof(ImageVotingsPermissions.DeleteOwnImageVotingEntry))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [EndpointSummary("Delete an image voting entry")]
    public async Task<ActionResult> DeleteImageVotingEntry([FromRoute] int id)
    {
        await Mediator.Send(new DeleteImageVotingEntry.Command
        {
            Id = id
        });

        return NoContent();
    }

    [HttpPost("{id}/Choose")]
    [Permissions(typeof(ImageVotingsPermissions.ChooseActiveImageVotingEntry),
        typeof(ImageVotingsPermissions.ChooseImageVotingEntry))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [EndpointSummary("Choose an image voting entry (single choice image votings only)")]
    public async Task<ActionResult> ChooseImageVotingEntry([FromRoute] int id,
        [FromBody] ChooseImageVotingEntry.RequestBody body)
    {
        await Mediator.Send(new ChooseImageVotingEntry.Command
        {
            Id = id,
            Body = body
        });

        return NoContent();
    }

    [HttpPost("{id}/Unchoose")]
    [Permissions(typeof(ImageVotingsPermissions.UnchooseActiveImageVotingEntry),
        typeof(ImageVotingsPermissions.UnchooseImageVotingEntry))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [EndpointSummary("Unchoose an image voting entry (single choice image votings only)")]
    public async Task<ActionResult> UnchooseImageVotingEntry([FromRoute] int id,
        [FromBody] UnchooseImageVotingEntry.RequestBody body)
    {
        await Mediator.Send(new UnchooseImageVotingEntry.Command
        {
            Id = id,
            Body = body
        });

        return NoContent();
    }
}