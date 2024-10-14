namespace ClassifiedsApi.Contracts.Contexts.Characteristics;

/// <summary>
/// Запрос на добавление характеристики объявления.
/// </summary>
public class CharacteristicAddRequest
{
    
    /// <summary>
    /// Идентификатор объявления.
    /// </summary>
    public required Guid AdvertId { get; init; }
    
    /// <summary>
    /// Модель добавления характеристики объявления <see cref="CharacteristicAdd"/>.
    /// </summary>
    public required CharacteristicAdd CharacteristicAdd { get; init; }
}