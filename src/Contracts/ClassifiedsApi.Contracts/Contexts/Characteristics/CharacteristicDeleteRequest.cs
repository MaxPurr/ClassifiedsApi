using ClassifiedsApi.Contracts.Contexts.Adverts;

namespace ClassifiedsApi.Contracts.Contexts.Characteristics;

/// <summary>
/// Модель пользовательского запроса на удаление харакатеристики объявления.
/// </summary>
public class CharacteristicDeleteRequest : AdvertRequest
{
    /// <summary>
    /// Идентификатор характеристики объявления.
    /// </summary>
    public required Guid CharacteristicId { get; init; }
}