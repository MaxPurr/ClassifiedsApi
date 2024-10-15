using System;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.AppServices.Exceptions.Categories;

namespace ClassifiedsApi.AppServices.Contexts.Categories.Validators;

/// <summary>
/// Валидатор категорий.
/// </summary>
public interface ICategoryValidator
{
    /// <summary>
    /// Проверяет, что категория с указанным идентификатором существует.
    /// </summary>
    /// <param name="id">Идентификатор категории.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns><code data-dev-comment-type="langword">true</code> если категория найдена, иначе <code data-dev-comment-type="langword">false</code>.</returns>
    Task<bool> ValidateExistsAsync(Guid id, CancellationToken token);
    
    /// <summary>
    /// Проверяет, что категория с указанным идентификатором существует и вызывает исключение <see cref="CategoryNotFoundException"/> если нет.
    /// </summary>
    /// <param name="id">Идентификатор категории.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns></returns>
    Task ValidateExistsAndThrowAsync(Guid id, CancellationToken token);
}