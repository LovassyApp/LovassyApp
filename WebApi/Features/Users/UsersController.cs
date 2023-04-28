using Microsoft.AspNetCore.Mvc;
using WebApi.Common.Models;
using WebApi.Features.Users.Commands;

namespace WebApi.Features.Users;

public class UsersController : ApiControllerBase
{
    [HttpPost]
    public async Task<ActionResult> CreateUser([FromBody] CreateUser.RequestBody body)
    {
        await Mediator.Send(new CreateUser.Command { Body = body });

        return Ok(); //TODO: Return created once there is a view endpoint
    }
}