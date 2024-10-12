using System.Net;
using ClassifiedsApi.AppServices.Exceptions.Common;
using ClassifiedsApi.Contracts.Common.Errors;

namespace ClassifiedsApi.AppServices.Exceptions.Comments;

/// <summary>
/// Исключение, возникающее когда комментарий был удален и операции с ним более недоступны.
/// </summary>
public class CommentHasBeenDeletedException : ApiException
{
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="CommentHasBeenDeletedException"/>.
    /// </summary>
    public CommentHasBeenDeletedException() 
        : base("Операции с комментарием более недоступны, поскольку он был удален.", HttpStatusCode.BadRequest)
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