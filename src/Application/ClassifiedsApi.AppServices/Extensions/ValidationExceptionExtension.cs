using System.Linq;
using System.Net;
using System.Text;
using ClassifiedsApi.Contracts.Common.Errors;
using FluentValidation;
using FluentValidation.Results;

namespace ClassifiedsApi.AppServices.Extensions;

/// <summary>
/// Расширения класса <see cref="ValidationException"/>.
/// </summary>
public static class ValidationExceptionExtension
{
    private static readonly string AttemptedValueMessageFormat = " Полученное значение: {0}";
    
    /// <summary>
    /// Метод для конвертации исключения <see cref="ValidationException"/> к модели ошибки валидации <see cref="ValidationApiError"/>.
    /// </summary>
    /// <param name="exception">Исключение.</param>
    /// <returns>Модель ошибки валидации <see cref="ValidationApiError"/>.</returns>
    public static ValidationApiError ToValidationApiError(this ValidationException exception)
    {
        var failures = exception.Errors.Select(FormatFailureMessage);
        return new ValidationApiError
        {
            Message = "Была допущена одна или несколько ошибок валидации.",
            Code = ((int)HttpStatusCode.BadRequest).ToString(),
            Failures = failures
        };
    }

    private static string FormatFailureMessage(ValidationFailure failure)
    {
        var sb = new StringBuilder(failure.ErrorMessage);
        var value = failure.AttemptedValue;
        if (!IsEmpty(value))
        {
            sb.AppendFormat(AttemptedValueMessageFormat, value);
        }
        return sb.ToString();
    }

    private static bool IsEmpty(object? value)
    {
        if (value is string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }
        return value == null;
    }
}