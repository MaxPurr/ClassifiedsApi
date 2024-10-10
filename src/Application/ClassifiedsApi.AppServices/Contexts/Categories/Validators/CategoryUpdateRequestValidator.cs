using ClassifiedsApi.AppServices.Contexts.Categories.Repositories;
using ClassifiedsApi.Contracts.Contexts.Categories;
using FluentValidation;

namespace ClassifiedsApi.AppServices.Contexts.Categories.Validators;

/// <summary>
/// Валидатор модели запроса на обновление категории.
/// </summary>
public class CategoryUpdateRequestValidator : AbstractValidator<CategoryRequest<CategoryUpdate>>
{
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="CategoryUpdateRequestValidator"/>.
    /// </summary>
    /// <param name="categoryRepository">Репозиторий категорий <see cref="ICategoryRepository"/>.</param>
    public CategoryUpdateRequestValidator(ICategoryRepository categoryRepository)
    {
        RuleFor(request => request.Model)
            .Cascade(CascadeMode.Stop)
            .Must(IsNotEmpty)
            .WithMessage("Модель обновления категории не может быть пустой.")
            .SetValidator(request => new CategoryUpdateValidator(request.CategoryId, categoryRepository));
    }
    
    private static bool IsNotEmpty(CategoryUpdate categoryUpdate)
    {
        return categoryUpdate.Name != null ||
               categoryUpdate.UpdateParentId != null;
    }
}