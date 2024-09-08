using ClassifiedsApi.Domain.Base;

namespace ClassifiedsApi.Domain.Entities;

/// <summary>
/// Модель категории.
/// </summary>
public class Category : BaseEntity { 
    /// <summary>
    /// Название.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Идентификатор родительской категории.
    /// </summary>
    public Guid? ParentId { get; set; }

    /// <summary>
    /// Родительская категория.
    /// </summary>
    public Category? ParentCategory { get; set; }

    /// <summary>
    /// Дочернии категории.
    /// </summary>
    public ICollection<Category> ChildCategories { get; set; }

    /// <summary>
    /// Объявления.
    /// </summary>
    public ICollection<Advert> Adverts { get; set; }
}