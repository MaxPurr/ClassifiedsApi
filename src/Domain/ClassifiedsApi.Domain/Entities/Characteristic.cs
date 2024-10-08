using System;
using ClassifiedsApi.Domain.Base;

namespace ClassifiedsApi.Domain.Entities;

/// <summary>
/// Модель характеристики объявления.
/// </summary>
public class Characteristic : BaseEntity
{
    /// <summary>
    /// Идентификатор объявления.
    /// </summary>
    public Guid AdvertId { get; set; }

    /// <summary>
    /// Объявление.
    /// </summary>
    public Advert Advert { get; set; } = null!;

    /// <summary>
    /// Название.
    /// </summary>
    public string Name { get; set; } = "";
    
    /// <summary>
    /// Значение.
    /// </summary>
    public string Value { get; set; } = "";
}