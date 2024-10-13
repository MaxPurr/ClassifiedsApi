using System.Net;

namespace ClassifiedsApi.Contracts.Common.Errors;

/// <summary>
/// Модель ошибки валидации.
/// </summary>
public class ValidationApiError : ApiError
{
    /// <summary>
    /// Список ошибок.
    /// </summary>
    public required IEnumerable<string> Failures { get; set; }
}