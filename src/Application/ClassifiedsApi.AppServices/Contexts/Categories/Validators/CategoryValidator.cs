using System;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.AppServices.Contexts.Categories.Repositories;
using ClassifiedsApi.AppServices.Exceptions.Categories;

namespace ClassifiedsApi.AppServices.Contexts.Categories.Validators;

/// <inheritdoc />
public class CategoryValidator : ICategoryValidator
{
    private readonly ICategoryRepository _repository;

    /// <summary>
    /// Инициализирует экземпляр класса <see cref="CategoryValidator"/>.
    /// </summary>
    /// <param name="repository">Репозиторий категорий <see cref="ICategoryRepository"/>.</param>
    public CategoryValidator(ICategoryRepository repository)
    {
        _repository = repository;
    }

    /// <inheritdoc />
    public Task<bool> ValidateExistsAsync(Guid id, CancellationToken token)
    {
        return _repository.IsExistsAsync(id, token);
    }

    /// <inheritdoc />
    public async Task ValidateExistsAndThrowAsync(Guid id, CancellationToken token)
    {
        var exists = await ValidateExistsAsync(id, token);
        if (!exists)
        {
            throw new CategoryNotFoundException();
        }
    }
}