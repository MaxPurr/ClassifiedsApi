using System;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.Contracts.Contexts.Characteristics;

namespace ClassifiedsApi.AppServices.Contexts.Characteristics.Services;

/// <summary>
/// Сервис характеристик объявлений.
/// </summary>
public interface ICharacteristicService
{
    /// <summary>
    /// Метод для добавления новой характеристики объявления.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="advertId">Идентификатор объявления.</param>
    /// <param name="characteristicAdd">Модель добавления характеристики объявления <see cref="CharacteristicAdd"/>.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns>Идентификатор новой характеристики объявления.</returns>
    Task<Guid> AddAsync(Guid userId, Guid advertId, CharacteristicAdd characteristicAdd, CancellationToken token);

    /// <summary>
    /// Метод для обновления характеристики объявления.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="advertId">Идентификатор объявления.</param>
    /// <param name="characteristicId">Идентификатор характеристики объявления.</param>
    /// <param name="characteristicUpdate">Модель обновления харакатеристики объявления <see cref="CharacteristicUpdate"/>.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns>Модель обновленной информации о характеристике объявления.</returns>
    Task<CharacteristicInfo> UpdateAsync(
        Guid userId, 
        Guid advertId, 
        Guid  characteristicId, 
        CharacteristicUpdate characteristicUpdate, 
        CancellationToken token);

    /// <summary>
    /// Метод для удаление характеристики объявления.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="advertId">Идентификатор объявления.</param>
    /// <param name="characteristicId">Идентификатор характеристики объявления.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns></returns>
    Task DeleteAsync(Guid userId, Guid advertId, Guid characteristicId, CancellationToken token);
}