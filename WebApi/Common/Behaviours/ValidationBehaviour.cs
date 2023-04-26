using FluentValidation;
using MediatR;
using ValidationException = WebApi.Common.Exceptions.ValidationException;

namespace WebApi.Common.Behaviours;

public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IServiceProvider _serviceProvider;

    public ValidationBehaviour(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        foreach (var prop in request.GetType().GetProperties())
        {
            var genericType = typeof(IValidator<>).MakeGenericType(prop.PropertyType);
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