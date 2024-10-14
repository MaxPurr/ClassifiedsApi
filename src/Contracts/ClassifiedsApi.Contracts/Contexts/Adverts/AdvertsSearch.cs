using ClassifiedsApi.Contracts.Common;

namespace ClassifiedsApi.Contracts.Contexts.Adverts;

/// <summary>
/// Модель поиска объявлений.
/// </summary>
public class AdvertsSearch : BasePagination
{
    /// <summary>
    /// Включать неактивные объявления.
    /// </summary>
    public bool? IncludeDisabled { get; set; }
    
    /// <summary>
    /// Модель поиска по тексту.
    /// </summary>
    public TextFilter? TextFilter { get; set; }
    
    /// <summary>
    /// Минимальная цена объявления.
    /// </summary>
    public decimal? MinPrice { get; set; }
    
    /// <summary>
    /// Максимальная цена объявления.
    /// </summary>
    public decimal? MaxPrice { get; set; }
    
    /// <summary>
    /// Фильтр по идентификатору категории.
    /// </summary>
    public Guid? FilterByCategoryId { get; set; }
    
    /// <summary>
    /// Модель сортировки объявлений.
    /// </summary>
    public AdvertsOrder? Order { get; set; }
}