using Microsoft.AspNetCore.Mvc;
using WebApi.Common.Models;
using WebApi.Features.Users.Commands;

namespace WebApi.Features.Users;

public class UsersController : ApiControllerBase
{
    [HttpPost]
    public async Task<ActionResult> CreateUser([FromBody] CreateUserBody body)
    {
        await Mediator.Send(new CreateUserCommand { Body = body });

        return Ok(); //TODO: Return created once there is a view endpoint
    }
}