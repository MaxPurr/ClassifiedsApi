using ClassifiedsApi.Domain.Base;

namespace ClassifiedsApi.Domain.Entities;

/// <summary>
/// Модель комментария к объявлению.
/// </summary>
public class Comment : BaseEntity {
    /// <summary>
    /// Текст.
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    /// Идентификатор родительского комментария.
    /// </summary>
    public Guid? ParentId { get; set; }

    /// <summary>
    /// Родительский комментарий.
    /// </summary>
    public Comment? ParrentComment { get; set; }

    /// <summary>
    /// Дочернии комментарии.
    /// </summary>
    public ICollection<Comment> ChildComments { get; set; }

    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// Пользователь.
    /// </summary>
    public User User { get; set; }

    /// <summary>
    /// Идентификатор объявления.
    /// </summary>
    public Guid AdvertId { get; set; }
    
    /// <summary>
    /// Обьявление.
    /// </summary>
    public Advert Advert { get; set; }
}