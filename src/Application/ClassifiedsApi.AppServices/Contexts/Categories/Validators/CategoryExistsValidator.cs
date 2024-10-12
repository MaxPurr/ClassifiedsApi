using System;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.AppServices.Common.Validators;
using ClassifiedsApi.AppServices.Contexts.Categories.Repositories;
using FluentValidation;

namespace ClassifiedsApi.AppServices.Contexts.Categories.Validators;

/// <summary>
/// Валидатор, проверяющий существует ли категория с указанным идентификатором.
/// </summary>

[IgnoreAutomaticRegistration]
public class CategoryExistsValidator : AbstractValidator<Guid?>
{
    private readonly ICategoryRepository _categoryRepository;
    
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="CategoryExistsValidator"/>.
    /// </summary>
    /// <param name="categoryRepository">Репозиторий категорий <see cref="ICategoryRepository"/>.</param>
    public CategoryExistsValidator(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
        RuleFor(id => id)
            .MustAsync(IsCategoryExistsAsync)
            .WithMessage("Категория с указанным идентификатором не найдена.");
    }
    
    private async Task<bool> IsCategoryExistsAsync(Guid? id, CancellationToken token)
    {
        if (id == null)
        {
            return false;
        }
        return await _categoryRepository.IsExistsAsync(id.GetValueOrDefault(), token);
    }
}