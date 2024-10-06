using System.Net;
using ClassifiedsApi.Contracts.Common.Errors;

namespace ClassifiedsApi.AppServices.Exceptions.Common;

/// <summary>
/// Исключение, возникающие при получении идентификатора с неверным форматом.
/// </summary>
public class InvalidObjectIdFormatException : ApiException
{
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="InvalidObjectIdFormatException"/>.
    /// </summary>
    public InvalidObjectIdFormatException() : base("Неверный формат идентификатора.", HttpStatusCode.BadRequest)
    {
        
    }

    public override ApiError ToApiError()
    {
        return new ApiError()
        {
            Message = Message,
            Code = ((int)HttpStatusCode.BadRequest).ToString()
        };
    }
}