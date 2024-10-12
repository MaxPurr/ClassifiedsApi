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
}