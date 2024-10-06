using System.Net;
using ClassifiedsApi.Contracts.Common.Errors;

namespace ClassifiedsApi.AppServices.Exceptions.Common;

/// <summary>
/// Исключение, возникающее когда искомая сущность не была найдена.
/// </summary>
public abstract class EntityNotFoundException : ApiException
{
    /// <summary>
    /// Инициализирует экземпляр <see cref="EntityNotFoundException"/>.
    /// </summary>
    /// <param name="message">Сообщение об ошибке.</param>
    protected EntityNotFoundException(string message) : base(message, HttpStatusCode.NotFound)
    {
        
    }
    
    /// <inheritdoc />
    public override ApiError ToApiError()
    {
        return new ApiError
        {
            Message = Message,
            Code = ((int)HttpStatusCode.NotFound).ToString(),
        };
    }
}