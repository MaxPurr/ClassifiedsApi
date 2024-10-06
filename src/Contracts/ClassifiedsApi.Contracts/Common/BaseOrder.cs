namespace ClassifiedsApi.Contracts.Common;

/// <summary>
/// Базовая модель сортировка.
/// </summary>
/// <typeparam name="TOrderBy">Тип поля, по которому производиться сортировка.</typeparam>
public abstract class BaseOrder<TOrderBy> where TOrderBy : Enum
{
    /// <summary>
    /// Поле, по которому производиться сортировка.
    /// </summary>
    public TOrderBy By { get; set; }
    
    /// <summary>
    /// По убыванию.
    /// </summary>
    public bool Descending { get; set; }
}