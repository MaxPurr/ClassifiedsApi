using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.Contracts.Contexts.Adverts;

namespace ClassifiedsApi.AppServices.Contexts.Adverts.Services;

/// <summary>
/// Сервис объявлений.
/// </summary>
public interface IAdvertService
{
    /// <summary>
    /// Метод для создания объявления.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="advertCreate">Модель создания объявления <see cref="AdvertCreate"/>.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns>Идентификатор созданного объявления.</returns>
    Task<Guid> CreateAsync(Guid userId, AdvertCreate advertCreate, CancellationToken token);
    
    /// <summary>
    /// Метод для получения объявления.
    /// </summary>
    /// <param name="id">Идентификатор объявления.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns>Модель информации об объявлении <see cref="AdvertInfo"/>.</returns>
    Task<AdvertInfo> GetByIdAsync(Guid id, CancellationToken token);
    
    /// <summary>
    /// Метод для поиска объявлений по запросу.
    /// </summary>
    /// <param name="search">Модель поиска объявлений <see cref="AdvertsSearch"/>.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns>Коллекция объявлений, удовлетворяющих запросу.</returns>
    Task<IReadOnlyCollection<ShortAdvertInfo>> SearchAsync(AdvertsSearch search, CancellationToken token);

    /// <summary>
    /// Метод для обновления объявления.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="advertId">Идентификатор объявления.</param>
    /// <param name="advertUpdate">Модель обновления объявления <see cref="AdvertUpdate"/>.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns>Модель обновленной информации об объявлении <see cref="AdvertInfo"/>.</returns>
    Task<AdvertInfo> UpdateAsync(Guid userId, Guid advertId, AdvertUpdate advertUpdate, CancellationToken token);
    
    /// <summary>
    /// Метод для удаления объявления.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="advertId">Идентификатор объявления.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns></returns>
    Task DeleteAsync(Guid userId, Guid advertId, CancellationToken token);
}