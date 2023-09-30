using FluentValidation;
using FluentValidation.Validators;

namespace Blueboard.Infrastructure.Files.Validators;

public class MaxFileSizeValidator<T, TProp> : PropertyValidator<T, TProp>
    where TProp : IFormFile, IEnumerable<IFormFile>
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
        switch (typeof(TProp))
        {
            case var type when typeof(IFormFile).IsAssignableFrom(type):
                return value.Length <= _maxFileSize;

            case var type when typeof(IEnumerable<IFormFile>).IsAssignableFrom(type):
                foreach (var file in value)
                    if (file.Length > _maxFileSize)
                        return false;
                break;
        }

        return true;
    }

    protected override string GetDefaultMessageTemplate(string errorCode)
    {
        return "A '{PropertyName}' nem lehet nagyobb, mint {MaxFileSize} byte";
    }
}