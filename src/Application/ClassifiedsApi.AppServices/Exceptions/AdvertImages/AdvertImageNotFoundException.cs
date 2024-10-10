using ClassifiedsApi.AppServices.Exceptions.Common;

namespace ClassifiedsApi.AppServices.Exceptions.AdvertImages;

/// <summary>
/// Исключение, возникающее когда искомая фотография объявления не была найдена.
/// </summary>
public class AdvertImageNotFoundException : EntityNotFoundException
{
    /// <summary>
    /// Инициализирует экземпляр <see cref="AdvertImageNotFoundException"/>.
    /// </summary>
    public AdvertImageNotFoundException() : base("Фотография объявления не была найдена.")
    {
        
    }
}