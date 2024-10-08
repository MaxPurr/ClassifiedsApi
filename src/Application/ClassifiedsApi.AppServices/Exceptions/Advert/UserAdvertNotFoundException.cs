using ClassifiedsApi.AppServices.Exceptions.Common;

namespace ClassifiedsApi.AppServices.Exceptions.Advert;

/// <summary>
/// Исключение, возникающее когда искомое объявление не было найдено среди объявлений пользователя.
/// </summary>
public class UserAdvertNotFoundException : EntityNotFoundException
{
    public UserAdvertNotFoundException() : base("Объявление не было найдено среди объявлений пользователя.")
    {
    }
}