namespace ClassifiedsApi.Contracts.Common.Requests;

/// <summary>
/// Базовая модель пользовательского запроса.
/// </summary>
public class UserRequest
{
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid UserId { get; set; }
}

/// <summary>
/// Типизированная модель пользовательского запроса.
/// </summary>
/// <typeparam name="TModel">Тип модели запроса.</typeparam>
public class UserRequest<TModel> : UserRequest where TModel : class
{
    /// <summary>
    /// Модель запроса.
    /// </summary>
    public required TModel Model { get; init; }
}