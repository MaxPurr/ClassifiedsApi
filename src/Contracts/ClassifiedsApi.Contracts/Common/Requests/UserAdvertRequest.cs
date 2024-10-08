namespace ClassifiedsApi.Contracts.Common.Requests;

/// <summary>
/// Базовая модель пользовательского запроса объявления.
/// </summary>
public class UserAdvertRequest
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
public class UserAdvertRequest<TModel> : UserAdvertRequest where TModel : class
{   
    /// <summary>
    /// Модель запроса.
    /// </summary>
    public required TModel Model { get; init; }
}