using System;
using System.Collections.Generic;
using ClassifiedsApi.Domain.Base;

namespace ClassifiedsApi.Domain.Entities;

/// <summary>
/// Модель обьявления.
/// </summary>
public class Advert : BaseEntity
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
    /// Идентификатор пользователя.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Пользователь.
    /// </summary>
    public User User { get; set; } = null!;

    /// <summary>
    /// Идентификатор категории.
    /// </summary>
    public Guid CategoryId { get; set; }

    /// <summary>
    /// Категория.
    /// </summary>
    public Category Category { get; set; } = null!;

    /// <summary>
    /// Характеристики объявления.
    /// </summary>
    public ICollection<Characteristic> Characteristics { get; set; } = null!;

    /// <summary>
    /// Коментарии.
    /// </summary>
    public ICollection<Comment> Comments { get; set; } = null!;
    
    /// <summary>
    /// Фотографии.
    /// </summary>
    public ICollection<AdvertImage> Images { get; set; } = null!;
    
    /// <summary>
    /// Неактивное объявление.
    /// </summary>
    public bool Disabled { get; set; }
    
    /// <summary>
    /// Пользователи, которым понравилось объявление.
    /// </summary>
    public ICollection<User> LikedUsers { get; set; } = null!;

    public ICollection<UserFavoriteAdvert> UserFavoriteAdverts { get; set; } = null!;
}