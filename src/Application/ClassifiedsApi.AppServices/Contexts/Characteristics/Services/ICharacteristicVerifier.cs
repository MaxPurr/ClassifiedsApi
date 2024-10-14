using System;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.AppServices.Exceptions.Characteristics;

namespace ClassifiedsApi.AppServices.Contexts.Characteristics.Services;

/// <summary>
/// Верификатор характеристик объявлений.
/// </summary>
public interface ICharacteristicVerifier
{
    /// <summary>
    /// Проверяет доступность названия для характеристики объявления и вызывает исключение <see cref="UnavailableCharacteristicNameException"/> если название недоступно.
    /// </summary>
    /// <param name="advertId">Идентификатор объявления.</param>
    /// <param name="characteristicName">Название характеристики.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns></returns>
    Task VerifyNameAvailabilityAndThrowAsync(Guid advertId, string characteristicName, CancellationToken token);
}