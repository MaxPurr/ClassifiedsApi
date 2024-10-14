using System.Net;
using ClassifiedsApi.AppServices.Exceptions.Common;
using ClassifiedsApi.Contracts.Common.Errors;

namespace ClassifiedsApi.AppServices.Exceptions.Users;

/// <summary>
/// Исключение, возникающее при попытки обращения к несуществующей фотографии пользователя.
/// </summary>
public class UserPhotoNotExistsException : ApiException
{
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="UserPhotoNotExistsException"/>.
    /// </summary>
    public UserPhotoNotExistsException() : base("У пользователя нет фотографии.", HttpStatusCode.BadRequest)
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