using System.Text.Json.Serialization;
using ClassifiedsApi.Contracts.Common;

namespace ClassifiedsApi.Contracts.Contexts.Adverts;

/// <summary>
/// Модель сортировки объявлений.
/// </summary>
public class AdvertsOrder : BaseOrder<AdvertsOrderBy> { }

/// <summary>
/// По какому полю сортировать объявления.
/// </summary>

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AdvertsOrderBy
{
    /// <summary>
    /// Не определено.
    /// </summary>
    None = 0,
    
    /// <summary>
    /// По идентификатору.
    /// </summary>
    Id = 1,
    
    /// <summary>
    /// По дате и времени создания.
    /// </summary>
    CreatedAt = 2,
    
    /// <summary>
    /// По названию.
    /// </summary>
    Title = 3,
}