using System;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.AppServices.Exceptions.Characteristics;

namespace ClassifiedsApi.AppServices.Contexts.Characteristics.Validators;

/// <summary>
/// Валидатор характеристик объявлений.
/// </summary>
public interface ICharacteristicValidator
{
    /// <summary>
    /// Проверяет доступность названия для характеристики объявления и вызывает исключение <see cref="UnavailableCharacteristicNameException"/> если название недоступно.
    /// </summary>
    /// <param name="advertId">Идентификатор объявления.</param>
    /// <param name="characteristicName">Название характеристики.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns></returns>
    Task ValidateNameAvailabilityAndThrowAsync(Guid advertId, string characteristicName, CancellationToken token);
}