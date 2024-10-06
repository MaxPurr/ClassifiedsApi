using System;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.AppServices.Contexts.Categories.Repositories;
using ClassifiedsApi.Contracts.Contexts.Categories;
using FluentValidation;

namespace ClassifiedsApi.AppServices.Contexts.Categories.Validators;

/// <summary>
/// Валидатор модели создания категории <see cref="CategoryCreate"/>.
/// </summary>
public class CategoryCreateValidator : AbstractValidator<CategoryCreate>
{
    private readonly ICategoryRepository _categoryRepository;
    
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="CategoryCreateValidator"/>.
    /// </summary>
    /// <param name="categoryRepository">Репозиторий категорий <see cref="ICategoryRepository"/>.</param>
    public CategoryCreateValidator(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
        RuleFor(categoryCreate => categoryCreate.Name)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(255);
        RuleFor(categoryCreate => categoryCreate.ParentId)
            .NotEqual(Guid.Empty)
            .MustAsync(async (id, token) => id == null || await IsCategoryExistsAsync(id.Value, token))
            .WithMessage("Родительская категория с указанным ID не найдена.");
    }

    private Task<bool> IsCategoryExistsAsync(Guid id, CancellationToken token)
    {
        return _categoryRepository.IsExistsAsync(id, token);
    }
}