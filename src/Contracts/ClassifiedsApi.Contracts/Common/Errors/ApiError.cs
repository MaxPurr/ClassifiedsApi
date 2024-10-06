using System.Net;

namespace ClassifiedsApi.Contracts.Common.Errors;

/// <summary>
/// Модель ошибки.
/// </summary>
public class ApiError
{
    /// <summary>
    /// Сообщение.
    /// </summary>
    public required string Message { get; init; }
    
    /// <summary>
    /// Код.
    /// </summary>
    public required string Code { get; init;  }
    
    /// <summary>
    /// Описание.
    /// </summary>
    public string? Description { get; init;  }
    
    /// <summary>
    /// Идентификатор запроса.
    /// </summary>
    public string? TraceId { get; set; }
}