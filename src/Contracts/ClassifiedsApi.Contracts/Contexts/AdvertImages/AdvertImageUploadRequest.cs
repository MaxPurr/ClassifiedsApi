using ClassifiedsApi.Contracts.Contexts.Adverts;
using ClassifiedsApi.Contracts.Contexts.Files;

namespace ClassifiedsApi.Contracts.Contexts.AdvertImages;

/// <summary>
/// Базовая модель пользовательского запроса на добавление фотографии объявления.
/// </summary>
public class AdvertImageUploadRequest : AdvertRequest
{
    /// <summary>
    /// Модель загрузки файла на сервер.
    /// </summary>
    public required FileUpload ImageUpload { get; set; }
}