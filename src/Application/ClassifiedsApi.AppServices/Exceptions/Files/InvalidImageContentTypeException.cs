using System.Net;
using ClassifiedsApi.AppServices.Exceptions.Common;
using ClassifiedsApi.Contracts.Common.Errors;

namespace ClassifiedsApi.AppServices.Exceptions.Files;

/// <summary>
/// Исключение, возникающее когда файл имеет неверный формат изображения.
/// </summary>
public class InvalidImageContentTypeException : ApiException
{
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="InvalidImageContentTypeException"/>.
    /// </summary>
    public InvalidImageContentTypeException() : base("Недопустимый тип файла изображения.", HttpStatusCode.BadRequest)
    {
        
    }

    /// <inheritdoc />
    public override ApiError ToApiError()
    {
        return new ApiError
        {
            Message = Message,
            Code = ((int)HttpStatusCode.BadRequest).ToString(),
        };
    }
}