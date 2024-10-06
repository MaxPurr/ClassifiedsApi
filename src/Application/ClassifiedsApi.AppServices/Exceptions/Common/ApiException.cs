using System;
using System.Net;
using ClassifiedsApi.Contracts.Common.Errors;

namespace ClassifiedsApi.AppServices.Exceptions.Common;

/// <summary>
/// Базовый класс API исключения.
/// </summary>
public abstract class ApiException : Exception
{
    /// <summary>
    /// Инициализирует экземпляр <see cref="ApiException"/>.
    /// </summary>
    /// <param name="message">Сообщение об ошибке.</param>
    /// <param name="statusCode">Код ошибки.</param>
    protected ApiException(string message, HttpStatusCode statusCode) : base(message)
    {
        StatusCode = statusCode;
    }
    
    /// <summary>
    /// Код ошибки.
    /// </summary>
    public HttpStatusCode StatusCode { get; }
    
    /// <summary>
    /// Метод для конвертации исключения к модели ошибки.
    /// </summary>
    /// <returns>Модель ошибки <see cref="ApiError"/>.</returns>
    public abstract ApiError ToApiError();
}