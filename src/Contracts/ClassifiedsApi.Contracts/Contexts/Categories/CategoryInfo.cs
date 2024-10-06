namespace ClassifiedsApi.Contracts.Contexts.Categories;

/// <summary>
/// Модель информации о категории.
/// </summary>
public class CategoryInfo
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Название.
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Идентификатор родительской категории.
    /// </summary>
    public Guid? ParentId { get; set; }
}