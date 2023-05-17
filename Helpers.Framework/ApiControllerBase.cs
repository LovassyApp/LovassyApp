using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Helpers.Framework;

/// <summary>
///     The base class for all API controllers.
/// </summary>
[ApiController]
[Route("Api/[controller]")]
[Produces("application/json")]
public abstract class ApiControllerBase : Controller
{
    private ISender? _mediator;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetService<ISender>()!;
}