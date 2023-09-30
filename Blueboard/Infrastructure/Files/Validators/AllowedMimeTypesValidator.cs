using FluentValidation;
using FluentValidation.Validators;

namespace Blueboard.Infrastructure.Files.Validators;

public class AllowedMimeTypesValidator<T, TProp> : PropertyValidator<T, TProp>
    where TProp : IFormFile, IEnumerable<IFormFile>
{
    private readonly string[] _mimeTypes;

    public AllowedMimeTypesValidator(string[] mimeTypes)
    {
        _mimeTypes = mimeTypes;
    }

    public override string Name { get; } = "AllowedMimeTypesValidator";

    public override bool IsValid(ValidationContext<T> context, TProp value)
    {
        switch (typeof(TProp))
        {
            case var type when typeof(IFormFile).IsAssignableFrom(type):
            {
                var mimeType = value.ContentType;
                if (!_mimeTypes.Contains(mimeType)) return false;
                break;
            }
            case var type when typeof(IEnumerable<IFormFile>).IsAssignableFrom(type):
            {
                foreach (var file in value)
                {
                    var mimeType = file.ContentType;
                    if (!_mimeTypes.Contains(mimeType)) return false;
                }

                break;
            }
        }

        return true;
    }

    protected override string GetDefaultMessageTemplate(string errorCode)
    {
        return "A '{PropertyName}' nem engedélyezett fájltípusú";
    }
}