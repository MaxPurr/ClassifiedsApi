namespace ClassifiedsApi.Domain.Base;

/// <summary>
/// Базовый класс для всех сущностей.
/// </summary>
public abstract class BaseEntity {
    /// <summary>
    /// Идентификатор записи.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Дата и время создания записи.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}