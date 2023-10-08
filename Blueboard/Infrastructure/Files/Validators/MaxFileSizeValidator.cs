using FluentValidation;
using FluentValidation.Validators;

namespace Blueboard.Infrastructure.Files.Validators;

public class MaxFileSizeValidator<T, TProp> : PropertyValidator<T, TProp>
    where TProp : IFormFile
{
    private readonly int _maxFileSize;

    public MaxFileSizeValidator(int maxFileSize)
    {
        _maxFileSize = maxFileSize;
    }

    public override string Name { get; } = "MaxFileSizeValidator";

    public override bool IsValid(ValidationContext<T> context, TProp value)
    {
        context.MessageFormatter.AppendArgument("MaxFileSize", _maxFileSize);

        return value?.Length <= _maxFileSize;
    }

    protected override string GetDefaultMessageTemplate(string errorCode)
    {
        return "A '{PropertyName}' nem lehet nagyobb, mint {MaxFileSize} byte";
    }
}