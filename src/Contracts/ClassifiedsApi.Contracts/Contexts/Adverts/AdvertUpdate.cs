namespace ClassifiedsApi.Contracts.Contexts.Adverts;

/// <summary>
/// Модель обновления объявления.
/// </summary>
public class AdvertUpdate
{
    /// <summary>
    /// Название.
    /// </summary>
    public string? Title { get; set; }
    
    /// <summary>
    /// Описание.
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// Цена.
    /// </summary>
    public decimal? Price { get; set; }
    
    /// <summary>
    /// Идентификатор категории.
    /// </summary>
    public Guid? CategoryId { get; set; }
    
    /// <summary>
    /// Неактивное объявление.
    /// </summary>
    public bool? Disabled { get; set; }
}