using ClassifiedsApi.AppServices.Exceptions.Files;

namespace ClassifiedsApi.AppServices.Contexts.Files.Services;

/// <summary>
/// Верификатор файлов.
/// </summary>
public interface IFileVerifier
{
    /// <summary>
    /// Верифицирует что тип контента файла соответствует изображению и вызывает исключение <see cref="InvalidImageContentTypeException"/> если нет.
    /// </summary>
    /// <param name="contentType">Тип контента.</param>
    void VerifyImageContentTypeAndThrow(string contentType);
}