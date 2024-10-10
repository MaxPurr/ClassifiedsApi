namespace ClassifiedsApi.Contracts.Contexts.Files;

/// <summary>
/// Модель загрузки файла на сервер.
/// </summary>
public class FileUpload
{
    /// <summary>
    /// Название.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Поток для чтения файла.
    /// </summary>
    public required Stream ReadStream { get; set; }

    /// <summary>
    /// Тип контента.
    /// </summary>
    public required string ContentType { get; set; }
}   