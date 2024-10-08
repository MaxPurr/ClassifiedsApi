using System;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.Contracts.Common.Requests;
using ClassifiedsApi.Contracts.Contexts.Adverts;

namespace ClassifiedsApi.AppServices.Contexts.Adverts.Repositories;

/// <summary>
/// Репозиторий объявлений.
/// </summary>
public interface IAdvertRepository
{
    /// <summary>
    /// Метод для создания объявления.
    /// </summary>
    /// <param name="advertCreateRequest">Модель пользовательского запроса на создание объявления.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns>Идентификатор созданного объявления.</returns>
    Task<Guid> CreateAsync(UserRequest<AdvertCreate> advertCreateRequest, CancellationToken token);
    
    /// <summary>
    /// Метод для получения объявления.
    /// </summary>
    /// <param name="id">Идентификатор объявления.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns>Модель информации об объявлении <see cref="AdvertInfo"/>.</returns>
    Task<AdvertInfo> GetByIdAsync(Guid id, CancellationToken token);
    
    /// <summary>
    /// Метод для обновления объявления.
    /// </summary>
    /// <param name="advertUpdateRequest">Модель пользовательского запроса на обновление объявления.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns>Модель обновленной информации об объявлении <see cref="AdvertInfo"/>.</returns>
    Task<AdvertInfo> UpdateAsync(UserAdvertRequest<AdvertUpdate> advertUpdateRequest, CancellationToken token);
    
    /// <summary>
    /// Метод для удаления объявления.
    /// </summary>
    /// <param name="id">Идентификатор объявления.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns></returns>
    Task DeleteAsync(Guid id, CancellationToken token);
    
    /// <summary>
    /// Метод для удаления объявления пользователем.
    /// </summary>
    /// <param name="advertId">Идентификатор объявления.</param>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns></returns>
    Task DeleteAsync(Guid advertId, Guid userId, CancellationToken token);
    
    /// <summary>
    /// Метод для проверки существования объявления.
    /// </summary>
    /// <param name="id">Идентификатор объявления <see cref="Guid"/>.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns><code data-dev-comment-type="langword">true</code> если объявление найдено, иначе <code data-dev-comment-type="langword">false</code>.</returns>
    Task<bool> IsExistsAsync(Guid id, CancellationToken token);
    
    /// <summary>
    /// Метод для проверки существования объявления среди объявлений пользователя. 
    /// </summary>
    /// <param name="advertId">Идентификатор объявления <see cref="Guid"/>.</param>
    /// <param name="userId">Идентификатор пользователя <see cref="Guid"/>.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns><code data-dev-comment-type="langword">true</code> если объявление найдено, иначе <code data-dev-comment-type="langword">false</code>.</returns>
    Task<bool> IsExistsAsync(Guid advertId, Guid userId, CancellationToken token);
}