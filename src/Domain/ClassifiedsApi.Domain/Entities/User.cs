using System;
using System.Collections.Generic;
using ClassifiedsApi.Domain.Base;

namespace ClassifiedsApi.Domain.Entities;

/// <summary>
/// Модель пользователя.
/// </summary>
public class User : BaseEntity {
    /// <summary>
    /// Логин.
    /// </summary>
    public string Login { get; set; } = "";

    /// <summary>
    /// Хэш пароля.
    /// </summary>
    public string PasswordHash { get; set; } = "";

    /// <summary>
    /// Имя.
    /// </summary>
    public string FirstName { get; set; } = "";

    /// <summary>
    /// Фамилия.
    /// </summary>
    public string LastName { get; set; } = "";

    /// <summary>
    /// Адрес электронной почты.
    /// </summary>
    public string Email { get; set; } = "";

    /// <summary>
    /// Верифицирована ли электронная почта.
    /// </summary>
    public bool EmailVerified { get; set; }

    /// <summary>
    /// Номер телефона.
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// Дата рождения.
    /// </summary>
    public DateTime BirthDate { get; set; }

    /// <summary>
    /// Идентификатор фотографии профиля.
    /// </summary>
    public Guid? PhotoId { get; set; }
    
    /// <summary>
    /// Роли пользователя.
    /// </summary>
    public ICollection<Role> Roles { get; set; } = null!;
    
    public ICollection<UserRole> UserRoles { get; set; } = null!;

    /// <summary>
    /// Объявления.
    /// </summary>
    public ICollection<Advert> Adverts { get; set; } = null!;

    /// <summary>
    /// Избранные объявления.
    /// </summary>
    public ICollection<Advert> LikedAdverts { get; set; } = null!;
    
    public ICollection<UserFavoriteAdvert> UserFavoriteAdverts { get; set; } = null!;
}