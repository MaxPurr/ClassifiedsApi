using ClassifiedsApi.AppServices.Exceptions.Files;

namespace ClassifiedsApi.AppServices.Contexts.Files.Validators;

/// <summary>
/// Валидатор файлов.
/// </summary>
public interface IFileValidator
{
    /// <summary>
    /// Проверяет, что тип контента файла соответствует изображению и вызывает исключение <see cref="InvalidImageContentTypeException"/> если нет.
    /// </summary>
    /// <param name="contentType">Тип контента.</param>
    void ValidateImageContentTypeAndThrow(string contentType);
}