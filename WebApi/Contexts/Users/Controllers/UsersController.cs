using Mapster;
using Microsoft.AspNetCore.Mvc;
using WebApi.Contexts.Users.Models;
using WebApi.Contexts.Users.Services;
using WebApi.Helpers.Cryptography.Exceptions;

namespace WebApi.Contexts.Users.Controllers;

[ApiController]
[Route("/Api/[controller]")]
[Produces("application/json")]
public class UsersController : Controller
{
    private readonly UsersService _usersService;

    public UsersController(UsersService usersService)
    {
        _usersService = usersService;
    }

    [HttpPost]
    public async Task<ActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
        if (!request.Email.EndsWith("lovassy.edu.hu"))
        {
            ModelState.AddModelError(nameof(CreateUserRequest.Email), "Only lovassy.edu.hu emails are allowed.");
            return ValidationProblem();
        }

        try
        {
            await _usersService.CreateUserAsync(request.Adapt<CreateUserDto>());
        }
        catch (ResetKeyPasswordMissingException e)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable);
        }

        return Ok(); //TODO: Return created once there is a view endpoint
    }
}