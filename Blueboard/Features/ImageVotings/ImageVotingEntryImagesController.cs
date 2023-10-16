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
public class ImageVotingEntryImagesController : ApiControllerBase
{
    [HttpGet]
    [Permissions(typeof(ImageVotingsPermissions.IndexOwnImageVotingEntryImages),
        typeof(ImageVotingsPermissions.IndexImageVotingEntryImages))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [EndpointSummary("List all images of an image voting")]
    public async Task<ActionResult<IEnumerable<IndexImageVotingEntryImages.Response>>> Index(
        [FromBody] IndexImageVotingEntryImages.RequestBody body,
        [FromQuery] SieveModel sieveModel)
    {
        var response = await Mediator.Send(new IndexImageVotingEntryImages.Query
        {
            Body = body,
            SieveModel = sieveModel
        });

        return Ok(response);
    }

    [HttpPost]
    [Permissions(typeof(ImageVotingsPermissions.UploadActiveImageVotingEntryImage),
        typeof(ImageVotingsPermissions.UploadImageVotingEntryImage))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [EndpointSummary("Upload an image to be used in an image voting entry")]
    public async Task<ActionResult<UploadImageVotingEntryImage.Response>> Create(
        [FromRoute] int imageVotingId,
        [FromForm] UploadImageVotingEntryImage.RequestBody body)
    {
        var response = await Mediator.Send(new UploadImageVotingEntryImage.Command
        {
            Body = body
        });

        return Ok(response);
    }

    [HttpDelete("{id}")]
    [Permissions(typeof(ImageVotingsPermissions.DeleteOwnImageVotingEntryImage),
        typeof(ImageVotingsPermissions.DeleteImageVotingEntryImage))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [EndpointSummary("Delete an image meant for an image voting entry")]
    public async Task<ActionResult> Delete(
        [FromRoute] int id)
    {
        await Mediator.Send(new DeleteImageVotingEntryImage.Command
        {
            Id = id
        });

        return NoContent();
    }
}