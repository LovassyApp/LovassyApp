using FluentValidation;
using FluentValidation.Validators;

namespace Blueboard.Infrastructure.Files.Validators;

public class AllowedMimeTypesValidator<T, TProp>(string[] mimeTypes) : PropertyValidator<T, TProp>
    where TProp : IFormFile
{
    public override string Name { get; } = "AllowedMimeTypesValidator";

    public override bool IsValid(ValidationContext<T> context, TProp value)
    {
        var mimeType = value?.ContentType;

        if (!mimeTypes.Contains(mimeType)) return false;

        return true;
    }

    protected override string GetDefaultMessageTemplate(string errorCode)
    {
        return "A '{PropertyName}' nem engedélyezett fájltípusú";
    }
}