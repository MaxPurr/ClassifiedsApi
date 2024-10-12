namespace ClassifiedsApi.Contracts.Contexts.Comments;

/// <summary>
/// Запрос на создание комментария.
/// </summary>
public class CommentCreateRequest
{
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public required Guid UserId { get; init; }
    
    /// <summary>
    /// Идентификатор объявления.
    /// </summary>
    public required Guid AdvertId { get; init; }
    
    /// <summary>
    /// Модель создания комментария.
    /// </summary>
    public required CommentCreate CommentCreate { get; init; }
}