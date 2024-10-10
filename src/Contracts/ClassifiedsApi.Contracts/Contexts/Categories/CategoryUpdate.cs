namespace ClassifiedsApi.Contracts.Contexts.Categories;

/// <summary>
/// Модель обновления категории.
/// </summary>
public class CategoryUpdate
{
    /// <summary>
    /// Новое название.
    /// </summary>
    public string? Name { get; set; }
    
    /// <summary>
    /// Модель обновления родительской категории.
    /// </summary>
    public UpdateParentId? UpdateParentId { get; set; }
}

/// <summary>
/// Модель обновления родительской категории.
/// </summary>
public class UpdateParentId
{
    /// <summary>
    /// Новый идентификатор родительской категории.
    /// </summary>
    public Guid? ParentId { get; set; } = Guid.Empty;
}