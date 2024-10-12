using System;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.AppServices.Exceptions.Advert;

namespace ClassifiedsApi.AppServices.Contexts.Adverts.Services;

/// <summary>
/// Верификатор объявлений.
/// </summary>
public interface IAdvertVerifier
{
    /// <summary>
    /// Проверяет что объявление существует и вызывает исключение <see cref="AdvertNotFoundException"/> если нет.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task VerifyExistsAndThrowAsync(Guid id, CancellationToken token);
}