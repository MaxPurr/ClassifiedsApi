namespace ClassifiedsApi.Contracts.Common;

/// <summary>
/// Базовая модель пагинации.
/// </summary>
public abstract class BasePagination
{
    /// <summary>
    /// Количество элементов для пропуска.
    /// </summary>
    public int? Skip { get; set; }
    
    /// <summary>
    /// Количество элементов для получения.
    /// </summary>
    public int Take { get; set; }
}