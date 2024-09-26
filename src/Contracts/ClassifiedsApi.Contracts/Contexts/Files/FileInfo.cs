namespace ClassifiedsApi.Contracts.Contexts.Files;

/// <summary>
/// Модель информации о файле.
/// </summary>
public class FileInfo
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public string Id { get; set; } = "";

    /// <summary>
    /// Дата и время создания.
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Название.
    /// </summary>
    public string Name { get; set; } = "";
    
    /// <summary>
    /// Размер файла.
    /// </summary>
    public long Length { get; set; }
}