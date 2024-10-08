using ClassifiedsApi.AppServices.Exceptions.Common;

namespace ClassifiedsApi.AppServices.Exceptions.Characteristics;

/// <summary>
/// Исключение, возникающее когда искомая характеристика объявления не была найдена.
/// </summary>
public class CharacteristicNotFoundException : EntityNotFoundException
{
    /// <summary>
    /// Инициализирует экземпляр <see cref="CharacteristicNotFoundException"/>.
    /// </summary>
    public CharacteristicNotFoundException() : base("Характеристика объявления не была найдена.")
    {
        
    }
}