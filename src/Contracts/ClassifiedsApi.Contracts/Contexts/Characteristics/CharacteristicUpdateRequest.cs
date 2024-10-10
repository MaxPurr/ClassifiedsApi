using ClassifiedsApi.Contracts.Contexts.Adverts;

namespace ClassifiedsApi.Contracts.Contexts.Characteristics;

/// <summary>
/// Модель пользовательского запроса на обновление харакатеристики объявления.
/// </summary>
public class CharacteristicUpdateRequest : AdvertRequest<CharacteristicUpdate>
{
    /// <summary>
    /// Идентификатор характеристики объявления.
    /// </summary>
    public required Guid CharacteristicId { get; init; }
}