namespace ClassifiedsApi.Contracts.Common;

/// <summary>
/// Модель фильтрации сущности по идентификатору родителя.
/// </summary>
public class FilterByParentId
{
    /// <summary>
    /// Идентификатор родителя.
    /// </summary>
    public Guid? ParentId { get; set; } = Guid.Empty;
}