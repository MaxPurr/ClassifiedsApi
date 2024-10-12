using System;
using ClassifiedsApi.AppServices.Contexts.Categories.Repositories;
using ClassifiedsApi.Contracts.Contexts.Categories;
using FluentValidation;

namespace ClassifiedsApi.AppServices.Contexts.Categories.Validators;

/// <summary>
/// Валидатор модели создания категории <see cref="CategoryCreate"/>.
/// </summary>
public class CategoryCreateValidator : AbstractValidator<CategoryCreate>
{
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="CategoryCreateValidator"/>.
    /// </summary>
    /// <param name="categoryRepository">Репозиторий категорий <see cref="ICategoryRepository"/>.</param>
    public CategoryCreateValidator(ICategoryRepository categoryRepository)
    {
        RuleFor(categoryCreate => categoryCreate.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(255);
        
        When(categoryCreate => categoryCreate.ParentId != null, () =>
        {
            RuleFor(categoryCreate => categoryCreate.ParentId)
                .Cascade(CascadeMode.Stop)
                .NotEqual(Guid.Empty)
                .SetValidator(new CategoryExistsValidator(categoryRepository));
        });
    }
}