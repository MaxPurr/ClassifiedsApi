namespace ClassifiedsApi.Contracts.Contexts.Adverts;

/// <summary>
/// Модель краткой информации об объявлении.
/// </summary>
public class ShortAdvertInfo
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
    /// Цена.
    /// </summary>
    public decimal Price { get; set; }
    
    /// <summary>
    /// Идентификаторы фотографий.
    /// </summary>
    public ICollection<Guid> ImageIds { get; set; } = null!;
    
    /// <summary>
    /// Неактивное объявление.
    /// </summary>
    public bool Disabled { get; set; }
    
    /// <summary>
    /// Дата и время создания.
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Идентификатор категории.
    /// </summary>
    public Guid CategoryId { get; set; }
    
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid UserId { get; set; }
}