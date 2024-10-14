using System;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.Contracts.Contexts.Users;

namespace ClassifiedsApi.AppServices.Contexts.Users.Repositories;

/// <summary>
/// Репозиторий пользователей.
/// </summary>
public interface IUserRepository
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
    /// <param name="photoId">Идентификатор фотографии.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns>Идентификатор предыдущей фотографии если есть, иначе null.</returns>
    Task<Guid?> UpdatePhotoAsync(Guid id, Guid photoId, CancellationToken token);
    
    /// <summary>
    /// Метод для удаления фотографии пользователя.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns>Идентификатор удаленной фотографии.</returns>
    Task<Guid> DeletePhotoAsync(Guid id, CancellationToken token);
    
    /// <summary>
    /// Проверяет сущетсвует ли пользователь с указаным идентификатором.
    /// </summary>
    /// <param name="id">Идентификатор пользователя.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns><code data-dev-comment-type="langword">true</code> если пользователь найден, иначе <code data-dev-comment-type="langword">false</code>.</returns>
    Task<bool> IsExistsAsync(Guid id, CancellationToken token);
    
    /// <summary>
    /// Проверяет сущетсвует ли пользователь с указаным логином.
    /// </summary>
    /// <param name="login">Логин пользователя.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns><code data-dev-comment-type="langword">true</code> если пользователь найден, иначе <code data-dev-comment-type="langword">false</code>.</returns>
    Task<bool> IsExistsAsync(string login, CancellationToken token);
}