using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Common.Models;

[ApiController]
[Route("Api/[controller]")]
[Produces("application/json")]
public abstract class ApiControllerBase : Controller
{
    private ISender? _mediator;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetService<ISender>()!;
}