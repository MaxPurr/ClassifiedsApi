using ClassifiedsApi.Contracts.Contexts.Characteristics;

namespace ClassifiedsApi.Contracts.Contexts.Adverts;

/// <summary>
/// Модель информации об объявлении.
/// </summary>
public class AdvertInfo
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Название.
    /// </summary>
    public string Title { get; set; } = "";
    
    /// <summary>
    /// Описание.
    /// </summary>
    public string Description { get; set; } = "";

    /// <summary>
    /// Характеристики.
    /// </summary>
    public ICollection<CharacteristicInfo> Characteristics { get; set; } = null!;
    
    /// <summary>
    /// Цена.
    /// </summary>
    public decimal Price { get; set; }
    
    /// <summary>
    /// Идентификаторы фотографий.
    /// </summary>
    public ICollection<string> ImageIds { get; set; } = null!;
    
    /// <summary>
    /// Идентификатор категории.
    /// </summary>
    public Guid CategoryId { get; set; }
    
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid UserId { get; set; }
}