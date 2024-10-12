using ClassifiedsApi.Contracts.Common;

namespace ClassifiedsApi.Contracts.Contexts.Comments;

/// <summary>
/// Модель поиска комментариев.
/// </summary>
public class CommentsSearch : BasePagination
{
    /// <summary>
    /// Модель фильтрации по идентификатору родительского комментария.
    /// </summary>
    public FilterByParentId? FilterByParentId { get; set; }
    
    /// <summary>
    /// Модель сортировки комментариев.
    /// </summary>
    public CommentsOrder? Order { get; set; }
}