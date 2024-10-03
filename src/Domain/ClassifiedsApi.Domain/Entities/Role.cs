using System.Collections.Generic;
using ClassifiedsApi.Domain.Base;

namespace ClassifiedsApi.Domain.Entities;

/// <summary>
/// Модель роли.
/// </summary>
public class Role : BaseEntity
{
    /// <summary>
    /// Название.
    /// </summary>
    public string Name { get; set; } = "";
    
    /// <summary>
    /// Пользователи.
    /// </summary>
    public ICollection<User> Users { get; set; } = null!;
    
    public ICollection<UserRole> UserRoles { get; set; } = null!;
}