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
    [HttpGet("{imageVotingId}/Images")]
    [Permissions(typeof(ImageVotingsPermissions.IndexOwnImageVotingEntryImages),
        typeof(ImageVotingsPermissions.IndexImageVotingEntryImages))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [EndpointDescription("Lists all images of an image voting.")]
    public async Task<IEnumerable<IndexImageVotingEntryImages.Response>> IndexImageVotingEntryImages(
        [FromRoute] int imageVotingId,
        [FromQuery] SieveModel sieveModel)
    {
        return await Mediator.Send(new IndexImageVotingEntryImages.Query
        {
            ImageVotingId = imageVotingId,
            SieveModel = sieveModel
        });
    }


    [HttpPost("{imageVotingId}/Upload")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [EndpointDescription("Uploads an image to be used in an image voting entry.")]
    public async Task<UploadImageVotingEntryImage.Response> UploadImageVotingEntryImage(
        [FromRoute] int imageVotingId,
        [FromForm] UploadImageVotingEntryImage.RequestBody body)
    {
        return await Mediator.Send(new UploadImageVotingEntryImage.Command
        {
            ImageVotingId = imageVotingId,
            Body = body
        });
    }

    [HttpDelete("Images/{id}")]
    [Permissions(typeof(ImageVotingsPermissions.DeleteOwnImageVotingEntryImage),
        typeof(ImageVotingsPermissions.DeleteImageVotingEntryImage))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [EndpointDescription("Deletes an image meant for an image voting entry.")]
    public async Task<IActionResult> DeleteImageVotingEntryImage(
        [FromRoute] int id)
    {
        await Mediator.Send(new DeleteImageVotingEntryImage.Command
        {
            Id = id
        });

        return NoContent();
    }
}