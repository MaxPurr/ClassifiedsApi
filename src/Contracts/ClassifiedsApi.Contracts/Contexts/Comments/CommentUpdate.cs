namespace ClassifiedsApi.Contracts.Contexts.Comments;

/// <summary>
/// Модель обновления комментария.
/// </summary>
public class CommentUpdate
{
    /// <summary>
    /// Текст.
    /// </summary>
    public string? Text { get; set; }
}