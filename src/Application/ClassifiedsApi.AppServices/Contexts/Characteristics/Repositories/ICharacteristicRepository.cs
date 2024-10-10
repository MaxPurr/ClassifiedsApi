using System;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.Contracts.Contexts.Adverts;
using ClassifiedsApi.Contracts.Contexts.Characteristics;

namespace ClassifiedsApi.AppServices.Contexts.Characteristics.Repositories;

/// <summary>
/// Репозиторий характеристик объявлений.
/// </summary>
public interface ICharacteristicRepository
{
    /// <summary>
    /// Метод для добавления новой характеристики объявления.
    /// </summary>
    /// <param name="characteristicAddRequest">Модель пользовательского запроса на добавление характеристики объявления.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns>Идентификатор характеристики.</returns>
    Task<Guid> AddAsync(AdvertRequest<CharacteristicAdd> characteristicAddRequest, CancellationToken token);

    /// <summary>
    /// Метод для удаления характеристики объявления.
    /// </summary>
    /// <param name="advertId">Идентификатор объявления.</param>
    /// <param name="id">Идентификатор характеристики объявления.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns></returns>
    Task DeleteAsync(Guid advertId, Guid id, CancellationToken token);

    /// <summary>
    /// Метод для обновления характеристики объявления.
    /// </summary>
    /// <param name="advertId">Идентификатор объявления.</param>
    /// <param name="id">Идентификатор характеристики объявления.</param>
    /// <param name="characteristicUpdate">Модель обновления характеристики объявления <see cref="CharacteristicUpdate"/>.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns>Модель обновленной характеристики объявления.</returns>
    Task<CharacteristicInfo> UpdateAsync(Guid advertId, Guid id, CharacteristicUpdate characteristicUpdate, CancellationToken token);

    /// <summary>
    /// Проверяет существует ли характеристика объявления с указанным названием.
    /// </summary>
    /// <param name="advertId">Идентификатор объявления.</param>
    /// <param name="name">Название характеристики.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns><code data-dev-comment-type="langword">true</code> если характеристика объявления найдена, иначе <code data-dev-comment-type="langword">false</code>.</returns>
    Task<bool> IsExistsAsync(Guid advertId, string name, CancellationToken token);
}