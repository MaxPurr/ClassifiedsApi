using System;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.Contracts.Contexts.Files;
using ClassifiedsApi.Contracts.Contexts.Users;

namespace ClassifiedsApi.AppServices.Contexts.Users.Services;

/// <summary>
/// Сервис пользователей.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Метод для получения информации о пользователе.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns>Модель информации о пользователе <see cref="UserInfo"/>.</returns>
    Task<UserInfo> GetByIdAsync(Guid id, CancellationToken token);
    
    /// <summary>
    /// Метод для получения контактов пользователя.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns>Модель информации о контактах пользователя <see cref="UserContactsInfo"/>.</returns>
    Task<UserContactsInfo> GetContactsInfoAsync(Guid id, CancellationToken token);
    
    /// <summary>
    /// Метод для обновления фотографии пользователя.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    /// <param name="photoUpload">Модель загрузки файла на сервер <see cref="FileUpload"/>.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns>Идентификатор загруженной фотографии.</returns>
    Task<Guid> UpdatePhotoAsync(Guid id, FileUpload photoUpload, CancellationToken token);
    
    /// <summary>
    /// Метод для удаления фотографии пользователя.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns></returns>
    Task DeletePhotoAsync(Guid id, CancellationToken token);
}