using FluentValidation;
using MediatR;
using ValidationException = WebApi.Common.Exceptions.ValidationException;

namespace WebApi.Common.Behaviours;

public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IValidatorFactory _validatorFactory;

    public ValidationBehaviour(IValidatorFactory validatorFactory)
    {
        _validatorFactory = validatorFactory;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        foreach (var prop in request.GetType().GetProperties())
        {
            var validator = _validatorFactory.GetValidator(prop.PropertyType);

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