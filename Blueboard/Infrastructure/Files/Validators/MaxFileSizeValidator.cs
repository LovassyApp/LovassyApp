using FluentValidation;
using FluentValidation.Validators;

namespace Blueboard.Infrastructure.Files.Validators;

public class MaxFileSizeValidator<T, TProp>(int maxFileSize) : PropertyValidator<T, TProp>
    where TProp : IFormFile
{
    public override string Name { get; } = "MaxFileSizeValidator";

    public override bool IsValid(ValidationContext<T> context, TProp value)
    {
        context.MessageFormatter.AppendArgument("MaxFileSize", maxFileSize);

        return value?.Length <= maxFileSize;
    }

    protected override string GetDefaultMessageTemplate(string errorCode)
    {
        return "A '{PropertyName}' nem lehet nagyobb, mint {MaxFileSize} byte";
    }
}