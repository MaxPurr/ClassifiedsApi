namespace ClassifiedsApi.Contracts.Contexts.Adverts;

/// <summary>
/// Базовая модель пользовательского запроса объявления.
/// </summary>
public class AdvertRequest
{
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public required Guid UserId { get; init; }
    
    /// <summary>
    /// Идентификатор объявления.
    /// </summary>
    public required Guid AdvertId { get; init; }
}

/// <summary>
/// Типизированная модель пользовательского запроса объявления.
/// </summary>
/// <typeparam name="TModel">Тип модели запроса.</typeparam>
public class AdvertRequest<TModel> : AdvertRequest where TModel : class
{   
    /// <summary>
    /// Модель запроса.
    /// </summary>
    public required TModel Model { get; init; }
}