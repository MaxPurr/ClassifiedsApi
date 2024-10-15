using System;

namespace ClassifiedsApi.AppServices.Common.Services;

/// <summary>
/// Сервис структурного логирования.
/// </summary>
public interface IStructuralLoggingService
{
    /// <summary>
    /// Метод для добавления свойства ко всем логам.
    /// </summary>
    /// <param name="name">Наименование свойства.</param>
    /// <param name="value">Значение свойства.</param>
    /// <param name="destructureObjects">Нужно ли раскладывать объект на поля.</param>
    /// <returns>Объект <see cref="IDisposable"/>.</returns>
    IDisposable PushProperty(string name, object value, bool destructureObjects = false);
}