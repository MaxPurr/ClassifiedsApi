using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ClassifiedsApi.AppServices.Contexts.AdvertImages.Repositories;

/// <summary>
/// Репозиторий фотографий объявлений.
/// </summary>
public interface IAdvertImageRepository
{
    /// <summary>
    /// Метод для добавления фотографии объявления.
    /// </summary>
    /// <param name="advertId">Идентификатор объявления.</param>
    /// <param name="imageId">Идентификатор фотографии.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns></returns>
    Task AddAsync(Guid advertId, Guid imageId, CancellationToken token);
    
    /// <summary>
    /// Метод для удаления фотографии объявления.
    /// </summary>
    /// <param name="advertId">Идентификатор объявления.</param>
    /// <param name="imageId">Идентификатор фотографии.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns></returns>
    Task DeleteAsync(Guid advertId, Guid imageId, CancellationToken token);
    
    /// <summary>
    /// Метод для получения идентификаторов всех фотографий объявления.
    /// </summary>
    /// <param name="advertId">Идентификатор объявления.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns>Список идентификаторов фотографий.</returns>
    Task<IReadOnlyCollection<Guid>> GetByAdvertIdAsync(Guid advertId, CancellationToken token);
    
    /// <summary>
    /// Метод для удаления всей фотографий объявления.
    /// </summary>
    /// <param name="advertId">Идентификатор объявления.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns>Список идентификаторов удаленных фотографий.</returns>
    Task<IReadOnlyCollection<Guid>> DeleteByAdvertIdAsync(Guid advertId, CancellationToken token);
}