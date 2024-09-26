namespace ClassifiedsApi.Contracts.Contexts.Files;

/// <summary>
/// Модель скачивания файла с сервера.
/// </summary>
public class FileDownload
{
    /// <summary>
    /// Название.
    /// </summary>
    public string Name { get; set; } = "";
    
    /// <summary>
    /// Поток для чтения файла.
    /// </summary>
    public Stream ReadStream { get; set; } = null!;
    
    /// <summary>
    /// Тип контента.
    /// </summary>
    public string ContentType { get; set; } = "";
}