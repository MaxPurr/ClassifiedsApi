using System.Text.Json.Serialization;
using ClassifiedsApi.Contracts.Common;

namespace ClassifiedsApi.Contracts.Contexts.Comments;

/// <summary>
/// Модель сортировки комментариев.
/// </summary>
public class CommentsOrder : BaseOrder<CommentsOrderBy> { }

/// <summary>
/// По какому полю сортировать объявления.
/// </summary>

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CommentsOrderBy
{
    /// <summary>
    /// Не определено.
    /// </summary>
    None = 0,
    
    /// <summary>
    /// По идентификатору.
    /// </summary>
    Id = 1,
    
    /// <summary>
    /// По дате и времени создания.
    /// </summary>
    CreatedAt = 2,
}