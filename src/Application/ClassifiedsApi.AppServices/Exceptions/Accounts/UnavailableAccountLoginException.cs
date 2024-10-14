using System.Net;
using ClassifiedsApi.AppServices.Exceptions.Common;
using ClassifiedsApi.Contracts.Common.Errors;

namespace ClassifiedsApi.AppServices.Exceptions.Accounts;

/// <summary>
/// Исключение, возникающее когда логин для аккаунта недоступен.
/// </summary>
public class UnavailableAccountLoginException : ApiException
{
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="UnavailableAccountLoginException"/>.
    /// </summary>
    public UnavailableAccountLoginException() 
        : base("Пользователь с таким логином уже существует.", HttpStatusCode.Conflict)
    {
        
    }

    /// <inheritdoc />
    public override ApiError ToApiError()
    {
        return new ApiError()
        {
            Message = Message,
            Code = ((int)HttpStatusCode.Conflict).ToString(),
        };
    }
}