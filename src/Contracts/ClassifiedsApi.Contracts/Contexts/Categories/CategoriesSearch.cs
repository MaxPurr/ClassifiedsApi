using ClassifiedsApi.Contracts.Common;

namespace ClassifiedsApi.Contracts.Contexts.Categories;

/// <summary>
/// Модель поиска категорий.
/// </summary>
public class CategoriesSearch : BasePagination
{
    /// <summary>
    /// Модель поиска по имени.
    /// </summary>
    public TextFilter? NameFilter { get; set; }
    
    /// <summary>
    /// Модель фильтрации по идентификатору родительской категории.
    /// </summary>
    public FilterByParentId? FilterByParentId { get; set; }
    
    /// <summary>
    /// Модель сортировки категорий.
    /// </summary>
    public CategoriesOrder? Order { get; set; }
}