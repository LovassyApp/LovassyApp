using Blueboard.Core.Auth.Permissions;
using Blueboard.Core.Auth.Policies.EmailConfirmed;
using Blueboard.Core.Auth.Policies.Permissions;
using Blueboard.Features.ImageVotings.Commands;
using Blueboard.Features.ImageVotings.Queries;
using Helpers.WebApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;

namespace Blueboard.Features.ImageVotings;

[Authorize]
[EmailVerified]
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

    [HttpPost("{imageVotingId}")]
    [Permissions(typeof(ImageVotingsPermissions.CreateImageVotingEntry),
        typeof(ImageVotingsPermissions.CreateActiveImageVotingEntry))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [EndpointSummary("Create an image voting entry")]
    public async Task<ActionResult<CreateImageVotingEntry.Response>> CreateImageVotingEntry(
        [FromRoute] int imageVotingId,
        [FromBody] CreateImageVotingEntry.RequestBody body)
    {
        var response = await Mediator.Send(new CreateImageVotingEntry.Command
        {
            ImageVotingId = imageVotingId,
            Body = body
        });

        return CreatedAtAction(nameof(View), new { id = response.Id }, response);
    }

    [HttpGet("{imageVotingId}/Images")]
    [Permissions(typeof(ImageVotingsPermissions.IndexOwnImageVotingEntryImages),
        typeof(ImageVotingsPermissions.IndexImageVotingEntryImages))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [EndpointSummary("List all images of an image voting")]
    public async Task<ActionResult<IEnumerable<IndexImageVotingEntryImages.Response>>> IndexImageVotingEntryImages(
        [FromRoute] int imageVotingId,
        [FromQuery] SieveModel sieveModel)
    {
        var response = await Mediator.Send(new IndexImageVotingEntryImages.Query
        {
            ImageVotingId = imageVotingId,
            SieveModel = sieveModel
        });

        return Ok(response);
    }


    [HttpPost("{imageVotingId}/Images")]
    [Permissions(typeof(ImageVotingsPermissions.UploadActiveImageVotingEntryImage),
        typeof(ImageVotingsPermissions.UploadImageVotingEntryImage))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [EndpointSummary("Upload an image to be used in an image voting entry")]
    public async Task<ActionResult<UploadImageVotingEntryImage.Response>> UploadImageVotingEntryImage(
        [FromRoute] int imageVotingId,
        [FromForm] UploadImageVotingEntryImage.RequestBody body)
    {
        var response = await Mediator.Send(new UploadImageVotingEntryImage.Command
        {
            ImageVotingId = imageVotingId,
            Body = body
        });

        return Ok(response);
    }

    [HttpDelete("Images/{id}")]
    [Permissions(typeof(ImageVotingsPermissions.DeleteOwnImageVotingEntryImage),
        typeof(ImageVotingsPermissions.DeleteImageVotingEntryImage))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [EndpointSummary("Delete an image meant for an image voting entry")]
    public async Task<ActionResult> DeleteImageVotingEntryImage(
        [FromRoute] int id)
    {
        await Mediator.Send(new DeleteImageVotingEntryImage.Command
        {
            Id = id
        });

        return NoContent();
    }
}