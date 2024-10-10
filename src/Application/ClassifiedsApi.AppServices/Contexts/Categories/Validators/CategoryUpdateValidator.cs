using System;
using ClassifiedsApi.AppServices.Common.Validators;
using ClassifiedsApi.AppServices.Contexts.Categories.Repositories;
using ClassifiedsApi.Contracts.Contexts.Categories;
using FluentValidation;

namespace ClassifiedsApi.AppServices.Contexts.Categories.Validators;

/// <summary>
/// Валидатор модели обновления категории <see cref="CategoryUpdate"/>.
/// </summary>

[IgnoreAutomaticRegistration]
public class CategoryUpdateValidator : AbstractValidator<CategoryUpdate>
{
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="CategoryUpdateValidator"/>.
    /// </summary>
    /// <param name="categoryId">Идентификатор категории.</param>
    /// <param name="categoryRepository">Репозиторий категорий <see cref="ICategoryRepository"/>.</param>
    public CategoryUpdateValidator(Guid categoryId, ICategoryRepository categoryRepository)
    {
        RuleFor(update => update.Name)
            .Cascade(CascadeMode.Stop)
            .MinimumLength(3)
            .MaximumLength(255);
        
        When(update => update.UpdateParentId != null &&
                       update.UpdateParentId.ParentId != null, () =>
        {
            RuleFor(update => update.UpdateParentId!.ParentId)
                .Cascade(CascadeMode.Stop)
                .NotEqual(Guid.Empty)
                .NotEqual(categoryId)
                .WithMessage("{PropertyName} не может быть равно идентификатору самой категории.")
                .SetValidator(new CategoryExistsValidator(categoryRepository))
                .WithName("Parent Id");
        });
    }
}