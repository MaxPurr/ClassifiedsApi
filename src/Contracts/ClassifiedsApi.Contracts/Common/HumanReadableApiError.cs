namespace ClassifiedsApi.Contracts.Common;

/// <summary>
/// Модель человеко-читаемой ошибки.
/// </summary>
public class HumanReadableApiError : ApiError
{
    /// <summary>
    /// Человеко-читаемое сообщение.
    /// </summary>
    public required String HumanReadableErrorMessage { get; init; }
}