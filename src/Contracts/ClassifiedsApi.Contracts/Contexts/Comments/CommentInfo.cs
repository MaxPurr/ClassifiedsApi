namespace ClassifiedsApi.Contracts.Contexts.Comments;

/// <summary>
/// Модель информации о комментарии.
/// </summary>
public class CommentInfo
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Текст.
    /// </summary>
    public string Text { get; set; } = "";
    
    /// <summary>
    /// Идентификатор родительского комментария.
    /// </summary>
    public Guid? ParentId { get; set; }
    
    /// <summary>
    /// Дата и время создания.
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Дата и время создания.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
    
    /// <summary>
    /// Дата и время удаления.
    /// </summary>
    public DateTime? DeletedAt { get; set; }
    
    /// <summary>
    /// Идентификатор объявления.
    /// </summary>
    public Guid AdvertId { get; set; }

    /// <summary>
    /// Модель информации о пользователе, оставившем комментарий.
    /// </summary>
    public CommentUserInfo User { get; set; } = null!;
}

/// <summary>
/// Модель информации о пользователе, оставившем комментарий.
/// </summary>
public class CommentUserInfo
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Имя.
    /// </summary>
    public string FirstName { get; set; }
    
    /// <summary>
    /// Фамилия.
    /// </summary>
    public string LastName { get; set; }
}