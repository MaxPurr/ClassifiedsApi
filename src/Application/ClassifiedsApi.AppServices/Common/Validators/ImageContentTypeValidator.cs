using System.Linq;
using ClassifiedsApi.Contracts.Contexts.Files;
using FluentValidation;

namespace ClassifiedsApi.AppServices.Common.Validators;

/// <summary>
/// Валидатор, проверяющий что файл имеет формат фотографии.
/// </summary>
public class ImageContentTypeValidator : AbstractValidator<FileUpload>
{
    private static readonly string[] ValidContentTypes = ["image/jpeg", "image/png"];
    
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="ImageContentTypeValidator"/>.
    /// </summary>
    public ImageContentTypeValidator()
    {
        RuleFor(imageUpload => imageUpload.ContentType)
            .Must(IsValidContentType)
            .WithMessage("Недопустимый тип файла изображения.");
    }

    private static bool IsValidContentType(string contentType)
    {
        return ValidContentTypes.Contains(contentType);
    }
}