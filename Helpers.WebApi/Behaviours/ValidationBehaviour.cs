using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using ValidationException = Helpers.WebApi.Exceptions.ValidationException;

namespace Helpers.WebApi.Behaviours;

public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IMemoryCache _memoryCache;
    private readonly IServiceProvider _serviceProvider;

    public ValidationBehaviour(IServiceProvider serviceProvider, IMemoryCache memoryCache)
    {
        _serviceProvider = serviceProvider;
        _memoryCache = memoryCache;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var cacheKey = $"{request.GetType().FullName}_Validation";

        var props = await _memoryCache.GetOrCreateAsync(cacheKey, entry =>
        {
            var properties = request.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            return Task.FromResult(properties);
        });

        foreach (var prop in props!)
        {
            var genericType = typeof(IValidator<>).MakeGenericType(prop.PropertyType); //Apparently this is pretty fast
            var validator = _serviceProvider.GetService(genericType) as IValidator;

            if (validator == null)
                continue;

            var type = typeof(ValidationContext<>).MakeGenericType(prop.PropertyType);
            var context = Activator.CreateInstance(type, prop.GetValue(request));

            var validationResult = await validator.ValidateAsync((IValidationContext)context!, cancellationToken);

            var failures = validationResult.Errors.Where(f => f != null).ToList();

            if (failures.Count != 0) throw new ValidationException(failures);
        }

        return await next();
    }
}