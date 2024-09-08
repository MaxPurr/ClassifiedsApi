using ClassifiedsApi.Domain.Base;

namespace ClassifiedsApi.Domain.Entities;

/// <summary>
/// Модель обьявления.
/// </summary>
public class Advert : BaseEntity {
    /// <summary>
    /// Название.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Описание.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Пользователь.
    /// </summary>
    public User User { get; set; }

    /// <summary>
    /// Идентификатор категории.
    /// </summary>
    public Guid CategoryId { get; set; }

    /// <summary>
    /// Категория.
    /// </summary>
    public Category Category { get; set; }

    /// <summary>
    /// Коментарии.
    /// </summary>
    public ICollection<Comment> Comments { get; set; }
}