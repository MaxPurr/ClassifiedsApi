using System;
using System.Net;
using ClassifiedsApi.Contracts.Common.Errors;

namespace ClassifiedsApi.AppServices.Exceptions.Common;

/// <summary>
/// Исключение, возникающее когда доступа к запрашиваемому ресурсу запрещен.
/// </summary>
public class ResourceAccessDeniedException : ApiException
{
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="ResourceAccessDeniedException"/>.
    /// </summary>
    /// <param name="message">Сообщение об ошибке.</param>
    public ResourceAccessDeniedException(string message) : base(message, HttpStatusCode.Forbidden)
    {
        
    }
    
    /// <inheritdoc />
    public override ApiError ToApiError()
    {
        return new ApiError
        {
            Message = Message,
            Code = ((int)HttpStatusCode.Forbidden).ToString(),
        };
    }
}