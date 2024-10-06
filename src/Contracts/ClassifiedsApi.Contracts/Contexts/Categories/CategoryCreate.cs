namespace ClassifiedsApi.Contracts.Contexts.Categories;

/// <summary>
/// Модель создания категории.
/// </summary>
public class CategoryCreate
{
    /// <summary>
    /// Название категории.
    /// </summary>
    public string? Name { get; set; }
    
    /// <summary>
    /// Идентификатор родительской категории.
    /// </summary>
    public Guid? ParentId { get; set; }
}