using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApi.Common.Exceptions;

namespace WebApi.Common.Filters;

public class ExceptionFilter : IExceptionFilter
{
    private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;
    private readonly ILogger<ExceptionFilter> _logger;

    public ExceptionFilter()
    {
        // Register known exception types and handlers.
        _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
        {
            { typeof(ValidationException), HandleValidationException },
            { typeof(NotFoundException), HandleNotFoundException },
            { typeof(UnauthorizedAccessException), HandleUnauthorizedAccessException },
            { typeof(ForbiddenException), HandleForbiddenException },
            { typeof(UnavailableException), HandleUnavailableException }
        };

        var loggerFactory = LoggerFactory.Create(loggingBuilder => loggingBuilder
            .SetMinimumLevel(LogLevel.Trace)
            .AddConsole());
        _logger = loggerFactory.CreateLogger<ExceptionFilter>();
    }

    public void OnException(ExceptionContext context)
    {
        HandleException(context);
    }

    private void HandleException(ExceptionContext context)
    {
        var type = context.Exception.GetType();
        if (_exceptionHandlers.ContainsKey(type))
        {
            _exceptionHandlers[type].Invoke(context);
            return;
        }

        if (!context.ModelState.IsValid)
        {
            HandleInvalidModelStateException(context);
            return;
        }

        HandleUnknownException(context);
    }

    private void HandleValidationException(ExceptionContext context)
    {
        var exception = context.Exception as ValidationException;

        ValidationProblemDetails details = new(exception!.Errors)
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
        };

        context.Result = new BadRequestObjectResult(details);

        context.ExceptionHandled = true;
    }

    private void HandleInvalidModelStateException(ExceptionContext context)
    {
        ValidationProblemDetails details = new(context.ModelState)
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
        };

        context.Result = new BadRequestObjectResult(details);

        context.ExceptionHandled = true;
    }

    private void HandleNotFoundException(ExceptionContext context)
    {
        var exception = context.Exception as NotFoundException;

        ProblemDetails details = new()
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            Title = "The specified resource was not found.",
            Detail = exception!.Message
        };

        context.Result = new NotFoundObjectResult(details);

        context.ExceptionHandled = true;
    }

    private void HandleUnauthorizedAccessException(ExceptionContext context)
    {
        ProblemDetails details = new()
        {
            Status = StatusCodes.Status401Unauthorized,
            Title = "Unauthorized",
            Type = "https://tools.ietf.org/html/rfc7235#section-3.1"
        };

        context.Result = new ObjectResult(details) { StatusCode = StatusCodes.Status401Unauthorized };

        context.ExceptionHandled = true;
    }

    private void HandleForbiddenException(ExceptionContext context)
    {
        ProblemDetails details = new()
        {
            Status = StatusCodes.Status403Forbidden,
            Title = "Forbidden",
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.3"
        };

        context.Result = new ObjectResult(details) { StatusCode = StatusCodes.Status403Forbidden };

        context.ExceptionHandled = true;
    }

    private void HandleUnavailableException(ExceptionContext context)
    {
        ProblemDetails details = new()
        {
            Status = StatusCodes.Status503ServiceUnavailable,
            Title = "Unavailable",
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.4"
        };

        context.Result = new ObjectResult(details) { StatusCode = StatusCodes.Status503ServiceUnavailable };

        context.ExceptionHandled = true;
    }

    private void HandleUnknownException(ExceptionContext context)
    {
        ProblemDetails details = new()
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "An error occurred while processing your request.",
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1"
        };

        _logger.LogError(context.Exception, "Unhandled {Exception} in request at path: '{Path}'",
            context.Exception.GetType(), context.HttpContext.Request.Path);

        context.Result = new ObjectResult(details) { StatusCode = StatusCodes.Status500InternalServerError };

        context.ExceptionHandled = true;
    }
}