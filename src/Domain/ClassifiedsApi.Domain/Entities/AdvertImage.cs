using System;

namespace ClassifiedsApi.Domain.Entities;

/// <summary>
/// Модель фотографии объявления.
/// </summary>
public class AdvertImage
{
    /// <summary>
    /// Идентификатор фотографии.
    /// </summary>
    public string ImageId { get; set; } = "";
    
    /// <summary>
    /// Идентификатор объявления.
    /// </summary>
    public Guid AdvertId { get; set; }

    /// <summary>
    /// Объявление.
    /// </summary>
    public Advert Advert { get; set; } = null!;
}