namespace ClassifiedsApi.Contracts.Contexts.Files;

/// <summary>
/// Модель загрузки файла на сервер.
/// </summary>
public class FileUpload
{
    /// <summary>
    /// Имя.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Контент.
    /// </summary>
    public required byte[] Content { get; set; }

    /// <summary>
    /// Тип контента.
    /// </summary>
    public required string ContentType { get; set; }
    
    /// <summary>
    /// Размер.
    /// </summary>
    public required long Length { get; set; }
}   