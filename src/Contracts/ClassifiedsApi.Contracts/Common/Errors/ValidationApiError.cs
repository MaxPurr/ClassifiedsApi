using System.Net;

namespace ClassifiedsApi.Contracts.Common.Errors;

/// <summary>
/// Модель ошибки валидации.
/// </summary>
public class ValidationApiError : ApiError
{
    public required IEnumerable<string> Failures { get; set; }
}