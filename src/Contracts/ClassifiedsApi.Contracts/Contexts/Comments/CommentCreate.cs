namespace ClassifiedsApi.Contracts.Contexts.Comments;

/// <summary>
/// Модель создания комментария.
/// </summary>
public class CommentCreate
{
    /// <summary>
    /// Текст комментария.
    /// </summary>
    public string? Text { get; set; }
    
    /// <summary>
    /// Идентификатор родительского комментария.
    /// </summary>
    public Guid? ParentId { get; set; }
}