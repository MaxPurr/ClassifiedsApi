namespace ClassifiedsApi.Contracts.Contexts.Files;

/// <summary>
/// Модель скачивания файла с сервера.
/// </summary>
public class FileDownload
{
    /// <summary>
    /// Имя.
    /// </summary>
    public string Name { get; set; } = "";
    
    /// <summary>
    /// Контент.
    /// </summary>
    public byte[] Content { get; set; } = null!;
    
    /// <summary>
    /// Тип контента.
    /// </summary>
    public string ContentType { get; set; } = "";
}