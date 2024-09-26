using System;
using ClassifiedsApi.Domain.Base;

namespace ClassifiedsApi.Domain.Entities;

/// <summary>
/// Модель фотографии объявления.
/// </summary>
public class AdvertImage : BaseEntity
{
    /// <summary>
    /// Идентификатор фотографии.
    /// </summary>
    public String ImageId { get; set; } = "";
    
    /// <summary>
    /// Идентификатор объявления.
    /// </summary>
    public Guid AdvertId { get; set; }

    /// <summary>
    /// Объявление.
    /// </summary>
    public Advert Advert { get; set; } = null!;
}