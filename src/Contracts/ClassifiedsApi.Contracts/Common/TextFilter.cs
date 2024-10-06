using System.Text.Json.Serialization;

namespace ClassifiedsApi.Contracts.Common;
/// <summary>
/// Модель поиска строки в тексте.
/// </summary>
public class TextFilter
{
    /// <summary>
    /// Искомая строка.
    /// </summary>
    public string? Query { get; set; } = "";

    /// <summary>
    /// Позиция строки в тексте.
    /// </summary>
    public QueryPosition QueryPosition { get; set; } = QueryPosition.None;
    
    /// <summary>
    /// Игнорировать регистр.
    /// </summary>
    public bool IgnoreCase { get; set; } = false;
}

/// <summary>
/// Позиция строки в тексте.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum QueryPosition
{
    /// <summary>
    /// Не определено.
    /// </summary>
    None = 0,

    /// <summary>
    /// В начале текста.
    /// </summary>
    Start = 1,

    /// <summary>
    /// В конце текста.
    /// </summary>
    End = 2,

    /// <summary>
    /// В любом месте текста.
    /// </summary>
    Anywhere = 3
}