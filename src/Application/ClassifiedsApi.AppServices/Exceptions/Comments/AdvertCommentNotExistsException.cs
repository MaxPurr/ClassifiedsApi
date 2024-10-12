using System.Net;
using ClassifiedsApi.AppServices.Exceptions.Common;
using ClassifiedsApi.Contracts.Common.Errors;

namespace ClassifiedsApi.AppServices.Exceptions.Comments;

/// <summary>
/// Исключение возникающее когда комментарий не был найден в объявлении.
/// </summary>
public class AdvertCommentNotExistsException : ApiException
{
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="AdvertCommentNotExistsException"/>.
    /// </summary>
    public AdvertCommentNotExistsException() 
        : base("Объявление не содержит комментария с указаным идентификатором", HttpStatusCode.BadRequest)
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