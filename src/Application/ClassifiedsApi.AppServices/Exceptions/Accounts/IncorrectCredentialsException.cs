using System.Net;
using ClassifiedsApi.AppServices.Exceptions.Common;
using ClassifiedsApi.Contracts.Common.Errors;

namespace ClassifiedsApi.AppServices.Exceptions.Accounts;

/// <summary>
/// Исключение, возникающее когда пользователь вводит неверные данные для входа в аккаунт.
/// </summary>
public class IncorrectCredentialsException : ApiException
{
    /// <summary>
    /// Инициализирует экземпляр <see cref="IncorrectCredentialsException"/>.
    /// </summary>
    public IncorrectCredentialsException() : 
        base("Неверные данные для входа в аккаунт.", HttpStatusCode.Unauthorized)
    {
        
    }
    
    /// <inheritdoc />
    public override ApiError ToApiError()
    {
        return new ApiError()
        {
            Message = "Неверный логин или пароль.",
            Code = ((int)HttpStatusCode.Unauthorized).ToString(),
        };
    }
}