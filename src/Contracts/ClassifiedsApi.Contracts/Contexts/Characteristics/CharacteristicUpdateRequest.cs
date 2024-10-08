using ClassifiedsApi.Contracts.Common.Requests;

namespace ClassifiedsApi.Contracts.Contexts.Characteristics;

/// <summary>
/// Модель пользовательского запроса на обновление харакатеристики объявления.
/// </summary>
public class CharacteristicUpdateRequest : UserAdvertRequest<CharacteristicUpdate>
{
    /// <summary>
    /// Идентификатор характеристики.
    /// </summary>
    public Guid CharacteristicId { get; set; }
}