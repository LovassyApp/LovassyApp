using Blueboard.Core.Auth.Policies.EmailConfirmed;
using Helpers.WebApi;
using Microsoft.AspNetCore.Authorization;

namespace Blueboard.Features.ImageVoting;

[Authorize]
[EmailVerified]
public class ImageVotingController : ApiControllerBase
{
}