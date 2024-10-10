using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.Contracts.Contexts.Categories;

namespace ClassifiedsApi.AppServices.Contexts.Categories.Services;

/// <summary>
/// Сервис категорий.
/// </summary>
public interface ICategoryService
{
    /// <summary>
    /// Метод для создания категории.
    /// </summary>
    /// <param name="categoryCreate">Модель создания категории <see cref="CategoryCreate"/>.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns>Идентификатор созданной категории <see cref="Guid"/>.</returns>
    Task<Guid> CreateAsync(CategoryCreate categoryCreate, CancellationToken token);
    
    /// <summary>
    /// Метод для получения информации о категории.
    /// </summary>
    /// <param name="id">Идентификатор категории <see cref="Guid"/>.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns>Модель информации о категории <see cref="CategoryInfo"/>.</returns>
    Task<CategoryInfo> GetInfoAsync(Guid id, CancellationToken token);
    
    /// <summary>
    /// Метод для поиска категорий по запросу.
    /// </summary>
    /// <param name="search">Модель поиска категорий <see cref="CategoriesSearch"/>.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns>Коллекция категорий, удовлетворяющих запросу.</returns>
    Task<IReadOnlyCollection<CategoryInfo>> SearchAsync(CategoriesSearch search, CancellationToken token);
    
    /// <summary>
    /// Метод для удаления категории.
    /// </summary>
    /// <param name="id">Идентификатор категории <see cref="Guid"/>.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns></returns>
    Task DeleteAsync(Guid id, CancellationToken token);
    
    /// <summary>
    /// Метод для обновления категории.
    /// </summary>
    /// <param name="updateRequest">Модель запроса на обновление категории.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns>Модель обновленной информации о категории <see cref="CategoryInfo"/>.</returns>
    Task<CategoryInfo> UpdateAsync(CategoryRequest<CategoryUpdate> updateRequest, CancellationToken token);
    
    /// <summary>
    /// Метод для проверки существования категории.
    /// </summary>
    /// <param name="id">Идентификатор категории <see cref="Guid"/>.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns><code data-dev-comment-type="langword">true</code> если категория найдена, иначе <code data-dev-comment-type="langword">false</code>.</returns>
    Task<bool> IsExistsAsync(Guid id, CancellationToken token);
}