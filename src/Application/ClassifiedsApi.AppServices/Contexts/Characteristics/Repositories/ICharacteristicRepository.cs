using System;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.Contracts.Common.Requests;
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
    /// <returns>Идентификатор новой характеристики объявления.</returns>
    Task<Guid> AddAsync(UserAdvertRequest<CharacteristicAdd> characteristicAddRequest, CancellationToken token);
    
    /// <summary>
    /// Метод для удаления характеристики объявления.
    /// </summary>
    /// <param name="characteristicDeleteRequest">Модель пользовательского запроса на удаление харакатеристики объявления <see cref="CharacteristicDeleteRequest"/>.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns></returns>
    Task DeleteAsync(CharacteristicDeleteRequest characteristicDeleteRequest, CancellationToken token);

    /// <summary>
    /// Метод для обновления характеристики объявления.
    /// </summary>
    /// <param name="characteristicUpdateRequest">Модель пользовательского запроса на обновление харакатеристики объявления <see cref="CharacteristicUpdateRequest"/>.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns>Модель обновленной информации о характеристике объявления.</returns>
    Task<CharacteristicInfo> UpdateAsync(CharacteristicUpdateRequest characteristicUpdateRequest, CancellationToken token);

    /// <summary>
    /// Проверяет существует ли характеристика объявления с указанным названием.
    /// </summary>
    /// <param name="advertId">Идентификатор объявления.</param>
    /// <param name="name">Название.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns><code data-dev-comment-type="langword">true</code> если характеристика объявления найдена, иначе <code data-dev-comment-type="langword">false</code>.</returns>
    Task<bool> IsExistsAsync(Guid advertId, string name, CancellationToken token);
}