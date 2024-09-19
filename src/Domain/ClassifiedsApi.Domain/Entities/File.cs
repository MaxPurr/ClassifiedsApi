using ClassifiedsApi.Domain.Base;

namespace ClassifiedsApi.Domain.Entities;

/// <summary>
/// Модель файла.
/// </summary>
public class File : BaseEntity
{
    /// <summary>
    /// Название.
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

    /// <summary>
    /// Размер файла.
    /// </summary>
    public int Length { get; set; }
}