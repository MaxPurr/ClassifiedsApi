using System.Linq;
using System.Net;
using ClassifiedsApi.Contracts.Common.Errors;
using FluentValidation;

namespace ClassifiedsApi.AppServices.Extensions;

/// <summary>
/// Расширения класса <see cref="ValidationException"/>.
/// </summary>
public static class ValidationExceptionExtension
{
    /// <summary>
    /// Метод для конвертации исключения <see cref="ValidationException"/> к модели ошибки валидации <see cref="ValidationApiError"/>.
    /// </summary>
    /// <param name="exception">Исключение.</param>
    /// <returns>Модель ошибки валидации <see cref="ValidationApiError"/>.</returns>
    public static ValidationApiError ToValidationApiError(this ValidationException exception)
    {
        var failures = exception.Errors.Select(failure => failure.ErrorMessage);
        return new ValidationApiError
        {
            Message = "Была допущена одна или несколько ошибок валидации.",
            Code = ((int)HttpStatusCode.BadRequest).ToString(),
            Failures = failures
        };
    }
}