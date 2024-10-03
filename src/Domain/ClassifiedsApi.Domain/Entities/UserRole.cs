using System;
using ClassifiedsApi.Domain.Base;

namespace ClassifiedsApi.Domain.Entities;

/// <summary>
/// Модель роли пользователя.
/// </summary>
public class UserRole : BaseEntity
{
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// Пользователь.
    /// </summary>
    public User User { get; set; } = null!;
    
    /// <summary>
    /// Идентификатор роли.
    /// </summary>
    public Guid RoleId { get; set; }
    
    /// <summary>
    /// Роль.
    /// </summary>
    public Role Role { get; set; } = null!;
}