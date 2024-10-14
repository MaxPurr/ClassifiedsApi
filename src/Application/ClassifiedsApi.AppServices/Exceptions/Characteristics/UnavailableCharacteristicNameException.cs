using System.Net;
using ClassifiedsApi.AppServices.Exceptions.Common;
using ClassifiedsApi.Contracts.Common.Errors;

namespace ClassifiedsApi.AppServices.Exceptions.Characteristics;

/// <summary>
/// Исключение, возникающее когда название для характеристики объявления недоступно.
/// </summary>
public class UnavailableCharacteristicNameException : ApiException
{
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="UnavailableCharacteristicNameException"/>.
    /// </summary>
    public UnavailableCharacteristicNameException() 
        : base("Характеристика объявления с таким названием уже существует.", HttpStatusCode.Conflict)
    {
        
    }
    
    /// <inheritdoc />
    public override ApiError ToApiError()
    {
        return new ApiError
        {
            Message = Message,
            Code = ((int)HttpStatusCode.Conflict).ToString(),
        };
    }
}