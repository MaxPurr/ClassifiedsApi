namespace ClassifiedsApi.Contracts.Contexts.Adverts;

/// <summary>
/// Модель информации об объявлении.
/// </summary>
public class AdvertInfo
{
    /// <summary>
    /// Название.
    /// </summary>
    public string Title { get; set; } = "";
    
    /// <summary>
    /// Описание.
    /// </summary>
    public string Description { get; set; } = "";
    
    /// <summary>
    /// Цена.
    /// </summary>
    public decimal Price { get; set; }
    
    /// <summary>
    /// Идентификаторы фотографий.
    /// </summary>
    public ICollection<Guid> PhotoIds { get; set; } = null!;
}