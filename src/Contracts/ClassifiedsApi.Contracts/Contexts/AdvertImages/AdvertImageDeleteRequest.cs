using ClassifiedsApi.Contracts.Contexts.Adverts;

namespace ClassifiedsApi.Contracts.Contexts.AdvertImages;

/// <summary>
/// Модель пользовательского запроса на удаление фотографии объявления.
/// </summary>
public class AdvertImageDeleteRequest : AdvertRequest
{
    /// <summary>
    /// Идентификатор фотографии.
    /// </summary>
    public required string ImageId { get; set; }
}