using System.Text.Json.Serialization;
using ClassifiedsApi.Contracts.Common;

namespace ClassifiedsApi.Contracts.Contexts.Categories;

/// <summary>
/// Модель сортировки категорий.
/// </summary>
public class CategoriesOrder : BaseOrder<CategoriesOrderBy> { }

/// <summary>
/// По какому полю сортировать категории.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CategoriesOrderBy
{
    /// <summary>
    /// Не определено.
    /// </summary>
    None = 0,
    
    /// <summary>
    /// По идентификатору.
    /// </summary>
    Id = 1,
    
    /// <summary>
    /// По имени.
    /// </summary>
    Name = 2,
    
    /// <summary>
    /// По идентификатору родительской категории.
    /// </summary>
    ParentId = 3,
}