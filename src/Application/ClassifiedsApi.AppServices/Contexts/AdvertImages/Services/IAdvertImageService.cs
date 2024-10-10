using System;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.Contracts.Contexts.AdvertImages;

namespace ClassifiedsApi.AppServices.Contexts.AdvertImages.Services;

/// <summary>
/// Сервис фотографий объявлений.
/// </summary>
public interface IAdvertImageService
{
    /// <summary>
    /// Метод для добавления фотографии объявления.
    /// </summary>
    /// <param name="imageUploadRequest">Модель пользовательского запроса на добавление фотографии объявления <see cref="AdvertImageUploadRequest"/>.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns>Идентификатор добавленной фотографии.</returns>
    Task<string> UploadAsync(AdvertImageUploadRequest imageUploadRequest, CancellationToken token);
    
    /// <summary>
    /// Метод для удаления фотографии объявления.
    /// </summary>
    /// <param name="imageDeleteRequest">Модель пользовательского запроса на удаление фотографии объявления <see cref="AdvertImageDeleteRequest"/>.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns></returns>
    Task DeleteAsync(AdvertImageDeleteRequest imageDeleteRequest, CancellationToken token);
    
    /// <summary>
    /// Метод для удалений всех фотографий объявления.
    /// </summary>
    /// <param name="advertId">Идентификатор объявления.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns></returns>
    Task DeleteAllAsync(Guid advertId, CancellationToken token);
}