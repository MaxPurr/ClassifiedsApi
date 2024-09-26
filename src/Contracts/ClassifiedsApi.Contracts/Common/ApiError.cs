namespace ClassifiedsApi.Contracts.Common;

/// <summary>
/// Модель ошибки.
/// </summary>
public class ApiError
{
    /// <summary>
    /// Сообщение.
    /// </summary>
    public required String Message { get; init; }
    
    /// <summary>
    /// Код.
    /// </summary>
    public required String Code { get; init; }

    /// <summary>
    /// Идентификатор запроса.
    /// </summary>
    public required String TraceId { get; init; }
    
    /// <summary>
    /// Описание.
    /// </summary>
    public String? Description { get; init; }
}