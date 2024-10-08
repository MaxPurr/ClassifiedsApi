using ClassifiedsApi.AppServices.Exceptions.Common;

namespace ClassifiedsApi.AppServices.Exceptions.Advert;

/// <summary>
/// Исключение, возникающее когда искомое объявление не было найдено.
/// </summary>
public class AdvertNotFoundException : EntityNotFoundException
{
    /// <summary>
    /// Инициализирует экземпляр <see cref="AdvertNotFoundException"/>.
    /// </summary>
    public AdvertNotFoundException() : base("Объявление не было найдено.")
    {
        
    }
}