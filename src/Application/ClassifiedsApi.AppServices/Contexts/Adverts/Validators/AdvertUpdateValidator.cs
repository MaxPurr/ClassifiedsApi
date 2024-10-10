using ClassifiedsApi.AppServices.Common.Validators;
using ClassifiedsApi.AppServices.Contexts.Categories.Repositories;
using ClassifiedsApi.AppServices.Contexts.Categories.Validators;
using ClassifiedsApi.Contracts.Contexts.Adverts;
using FluentValidation;

namespace ClassifiedsApi.AppServices.Contexts.Adverts.Validators;

/// <summary>
/// Валидатор модели обновления объявления <see cref="AdvertUpdate"/>.
/// </summary>

[IgnoreAutomaticRegistration]
public class AdvertUpdateValidator : AbstractValidator<AdvertUpdate>
{
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="AdvertUpdateValidator"/>.
    /// </summary>
    /// <param name="categoryRepository">Репозиторий категорий <see cref="ICategoryRepository"/>.</param>
    public AdvertUpdateValidator(ICategoryRepository categoryRepository)
    {
        When(update => update.Title != null, () =>
        {
            RuleFor(update => update.Title)
                .Cascade(CascadeMode.Stop)
                .MinimumLength(3)
                .MaximumLength(255);
        });

        When(update => update.Description != null, () =>
        {
            RuleFor(update => update.Description)
                .Cascade(CascadeMode.Stop)
                .MinimumLength(3)
                .MaximumLength(255);
        });

        When(update => update.Price != null, () =>
        {
            RuleFor(update => update.Price)
                .GreaterThanOrEqualTo(0);
        });

        When(update => update.CategoryId != null, () =>
        {
            RuleFor(update => update.CategoryId)
                .SetValidator(new CategoryExistsValidator(categoryRepository));
        });
    }
}