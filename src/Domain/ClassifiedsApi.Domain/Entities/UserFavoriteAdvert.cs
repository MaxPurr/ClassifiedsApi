using System;

namespace ClassifiedsApi.Domain.Entities;

/// <summary>
/// Модель избранного объявления.
/// </summary>
public class UserFavoriteAdvert
{
    /// <summary>
    /// Дата и время создания.
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Пользователь.
    /// </summary>
    public User User { get; set; } = null!;
    
    /// <summary>
    /// Идентификатор объявления.
    /// </summary>
    public Guid AdvertId { get; set; }
    
    /// <summary>
    /// Объявление.
    /// </summary>
    public Advert Advert { get; set; } = null!;
}