using System;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.AppServices.Exceptions.Advert;

namespace ClassifiedsApi.AppServices.Contexts.Adverts.Validators;

/// <summary>
/// Валидатор объявлений.
/// </summary>
public interface IAdvertValidator
{
    /// <summary>
    /// Проверяет, что объявление существует и вызывает исключение <see cref="AdvertNotFoundException"/> если нет.
    /// </summary>
    /// <param name="id">Идентификатор объявления.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns></returns>
    Task ValidateExistsAndThrowAsync(Guid id, CancellationToken token);
}