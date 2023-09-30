using Blueboard.Infrastructure.Files.Validators;
using FluentValidation;

namespace Blueboard.Infrastructure.Files.Extensions;

public static class RuleBuilderExtension
{
    /// <summary>
    ///     Limit the allowed mime types of the uploaded <see cref="IFormFile" /> or an enumerable of uploaded
    ///     <see cref="IFormFile">IFormFiles</see>.
    /// </summary>
    /// <param name="ruleBuilder">The rule builder for the param.</param>
    /// <param name="mimeTypes">The allowed mime types.</param>
    /// <typeparam name="T">The type of the object that is being validated.</typeparam>
    /// <typeparam name="TProp">The type of the property that is to be validated.</typeparam>
    /// <returns>An instance of <see cref="IRuleBuilderOptions{T,TProperty}" />.</returns>
    public static IRuleBuilderOptions<T, TProp> InMimeType<T, TProp>(this IRuleBuilder<T, TProp> ruleBuilder,
        string[] mimeTypes) where TProp : IFormFile, IEnumerable<IFormFile>
    {
        return ruleBuilder.SetValidator(new AllowedMimeTypesValidator<T, TProp>(mimeTypes));
    }

    /// <summary>
    ///     Limit the maximum file size of the uploaded <see cref="IFormFile" /> or an enumerable of uploaded
    ///     <see cref="IFormFile">IFormFiles</see> in bytes.
    /// </summary>
    /// <param name="ruleBuilder">The rule builder for the param.</param>
    /// <param name="maxFileSize">The max size of the file(s) in bytes.</param>
    /// <typeparam name="T">The type of the object that is being validated.</typeparam>
    /// <typeparam name="TProp">The type of the property that is to be validated.</typeparam>
    /// <returns>An instance of <see cref="IRuleBuilderOptions{T,TProperty}" />.</returns>
    public static IRuleBuilderOptions<T, TProp> MaxFileSize<T, TProp>(this IRuleBuilder<T, TProp> ruleBuilder,
        int maxFileSize) where TProp : IFormFile, IEnumerable<IFormFile>
    {
        return ruleBuilder.SetValidator(new MaxFileSizeValidator<T, TProp>(maxFileSize));
    }
}