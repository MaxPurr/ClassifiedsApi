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
    Task AddAsync(Guid advertId, string imageId, CancellationToken token);
    
    /// <summary>
    /// Метод для удаления фотографии объявления.
    /// </summary>
    /// <param name="advertId">Идентификатор объявления.</param>
    /// <param name="imageId">Идентификатор фотографии.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns></returns>
    Task DeleteAsync(Guid advertId, string imageId, CancellationToken token);
    
    /// <summary>
    /// Метод для получения списка идентификаторов фотографий объявления.
    /// </summary>
    /// <param name="advertId">Идентификатор объявления.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns>Список идентификаторов фотографий объявления.</returns>
    Task<IReadOnlyCollection<string>> GetAllAsync(Guid advertId, CancellationToken token);
}