using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.Contracts.Contexts.Files;

namespace ClassifiedsApi.AppServices.Contexts.AdvertImages.Services;

/// <summary>
/// Сервис фотографий объявлений.
/// </summary>
public interface IAdvertImageService
{
    /// <summary>
    /// Метод для добавления фотографии объявления.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="advertId">Идентификатор объявления.</param>
    /// <param name="imageUpload">Модель загрузки файла на сервер <see cref="FileUpload"/>.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns>Идентификатор добавленной фотографии.</returns>
    Task<Guid> UploadAsync(Guid userId, Guid advertId, FileUpload imageUpload, CancellationToken token);
    
    /// <summary>
    /// Метод для удаления фотографии объявления.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="advertId">Идентификатор объявления.</param>
    /// <param name="imageId">Идентификатор фотографии.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns></returns>
    Task DeleteAsync(Guid userId, Guid advertId, Guid imageId, CancellationToken token);
    
    /// <summary>
    /// Метод для получения идентификаторов всех фотографий объявления.
    /// </summary>
    /// <param name="advertId">Идентификатор объявления.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns>Список идентификаторов фотографий.</returns>
    Task<IReadOnlyCollection<Guid>> GetByAdvertIdAsync(Guid advertId, CancellationToken token);
    
    /// <summary>
    /// Метод для удаления всех фотографий объявления.
    /// </summary>
    /// <param name="advertId">Идентификатор объявления.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns></returns>
    Task DeleteByAdvertIdAsync(Guid advertId, CancellationToken token);
}