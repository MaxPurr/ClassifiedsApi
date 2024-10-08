using ClassifiedsApi.Contracts.Common.Requests;

namespace ClassifiedsApi.Contracts.Contexts.Characteristics;

/// <summary>
/// Модель пользовательского запроса на удаление харакатеристики объявления.
/// </summary>
public class CharacteristicDeleteRequest : UserAdvertRequest
{
    /// <summary>
    /// Идентификатор характеристики.
    /// </summary>
    public Guid CharacteristicId { get; set; }
}