using ClassifiedsApi.Contracts.Contexts.Categories;
using FluentValidation;

namespace ClassifiedsApi.AppServices.Contexts.Categories.Validators;

/// <summary>
/// Валидатор модели обновления категории <see cref="CategoryUpdate"/>.
/// </summary>
public class CategoryUpdateValidator : AbstractValidator<CategoryUpdate>
{
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="CategoryUpdateValidator"/>.
    /// </summary>
    public CategoryUpdateValidator()
    {
        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(255);
    }
}