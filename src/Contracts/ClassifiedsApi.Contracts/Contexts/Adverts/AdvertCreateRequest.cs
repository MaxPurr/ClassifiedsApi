namespace ClassifiedsApi.Contracts.Contexts.Adverts;

/// <summary>
/// Запрос на создание объявления.
/// </summary>
public class AdvertCreateRequest
{
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public required Guid UserId { get; init; }
    
    /// <summary>
    /// Модель создания объявления <see cref="AdvertCreate"/>.
    /// </summary>
    public required AdvertCreate AdvertCreate { get; init; }
}