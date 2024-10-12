using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.AppServices.Specifications;
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
    /// <param name="createRequest">Запрос на создание объявления <see cref="AdvertCreateRequest"/>.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns>Идентификатор созданного объявления.</returns>
    Task<Guid> CreateAsync(AdvertCreateRequest createRequest, CancellationToken token);
    
    /// <summary>
    /// Метод для получения объявления.
    /// </summary>
    /// <param name="id">Идентификатор объявления.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns>Модель информации об объявлении <see cref="AdvertInfo"/>.</returns>
    Task<AdvertInfo> GetByIdAsync(Guid id, CancellationToken token);
    
    /// <summary>
    /// Метод для получения объявлений по спецификации с пагинацией.
    /// </summary>
    /// <param name="specification">Спецификация <see cref="ISpecification{TEntity}"/>.</param>
    /// <param name="skip">Количество элементов для пропуска.</param>
    /// <param name="take">Количество элементов для получения.</param>
    /// <param name="order">Модель сортировки объявлений <see cref="AdvertsOrder"/>.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns>Колекция моделей краткой информации об объявлениях.</returns>
    Task<IReadOnlyCollection<ShortAdvertInfo>> GetBySpecificationWithPaginationAsync(
        ISpecification<ShortAdvertInfo> specification, 
        int? skip, 
        int take,
        AdvertsOrder order,
        CancellationToken token);

    /// <summary>
    /// Метод для обновления объявления.
    /// </summary>
    /// <param name="id">Идентификатор объявления.</param>
    /// <param name="advertUpdate">Модель обновления объявления <see cref="AdvertUpdate"/>.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns>Модель обновленной информации об объявлении <see cref="AdvertInfo"/>.</returns>
    Task<AdvertInfo> UpdateAsync(Guid id, AdvertUpdate advertUpdate, CancellationToken token);
    
    /// <summary>
    /// Метод для удаления объявления.
    /// </summary>
    /// <param name="id">Идентификатор объявления.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns></returns>
    Task DeleteAsync(Guid id, CancellationToken token);
    
    /// <summary>
    /// Проверяет существует ли объявление.
    /// </summary>
    /// <param name="id">Идентификатор объявления <see cref="Guid"/>.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns><code data-dev-comment-type="langword">true</code> если объявление найдено, иначе <code data-dev-comment-type="langword">false</code>.</returns>
    Task<bool> IsExistsAsync(Guid id, CancellationToken token);
    
    /// <summary>
    /// Метод для получения идентификатора пользователя объявления.
    /// </summary>
    /// <param name="id">Идентификатор объявления.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns>Идентификатор пользователя.</returns>
    Task<Guid> GetUserIdAsync(Guid id, CancellationToken token);
}