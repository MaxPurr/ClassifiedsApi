using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.AppServices.Specifications;
using ClassifiedsApi.Contracts.Contexts.Categories;

namespace ClassifiedsApi.AppServices.Contexts.Categories.Repositories;

/// <summary>
/// Репозиторий категорий.
/// </summary>
public interface ICategoryRepository
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
    /// Метод для получения категорий по спецификации с пагинацией.
    /// </summary>
    /// <param name="specification">Спецификация <see cref="ISpecification{TEntity}"/>.</param>
    /// <param name="skip">Количество элементов для пропуска.</param>
    /// <param name="take">Количество элементов для получения.</param>
    /// <param name="order">Модель сортировки категорий <see cref="CategoriesOrder"/>.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns>Колекция моделей информации о категориях.</returns>
    Task<IReadOnlyCollection<CategoryInfo>> GetBySpecificationWithPaginationAsync(
        ISpecification<CategoryInfo> specification, 
        int? skip, 
        int take,
        CategoriesOrder order,
        CancellationToken token);
    
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
    /// <param name="id">Идентификатор категории <see cref="Guid"/>.</param>
    /// <param name="categoryUpdate">Модель обновления категории <see cref="CategoryUpdate"/>.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns>Модель обновленной информации о категории <see cref="CategoryInfo"/>.</returns>
    Task<CategoryInfo> UpdateAsync(Guid id, CategoryUpdate categoryUpdate, CancellationToken token);
    
    /// <summary>
    /// Метод для проверки существования категории.
    /// </summary>
    /// <param name="id">Идентификатор категории <see cref="Guid"/>.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns><code data-dev-comment-type="langword">true</code> если категория найдена, иначе <code data-dev-comment-type="langword">false</code>.</returns>
    Task<bool> IsExistsAsync(Guid id, CancellationToken token);
}