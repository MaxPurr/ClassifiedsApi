using System;

namespace ClassifiedsApi.Domain.Base;

/// <summary>
/// Базовый класс для всех сущностей.
/// </summary>
public abstract class BaseEntity {
    /// Идентификатор.
    public Guid Id { get; set; }
    
    /// <summary>
    /// Дата и время создания сущности.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}