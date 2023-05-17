using Helpers.Framework;
using Microsoft.AspNetCore.Mvc;
using WebApi.Features.Users.Commands;

namespace WebApi.Features.Users;

public class UsersController : ApiControllerBase
{
    [HttpPost]
    public async Task<ActionResult> CreateUser([FromBody] CreateUser.RequestBody body, [FromQuery] string verifyUrl,
        [FromQuery] string verifyTokenQueryKey)
    {
        await Mediator.Send(new CreateUser.Command
            { Body = body, VerifyUrl = verifyUrl, VerifyTokenQueryKey = verifyTokenQueryKey });

        return Ok(); //TODO: Return created once there is a view endpoint
    }
}