using ClassifiedsApi.AppServices.Common.Validators;
using ClassifiedsApi.Contracts.Contexts.Categories;
using FluentValidation;

namespace ClassifiedsApi.AppServices.Contexts.Categories.Validators;

/// <summary>
/// Валидатор модели поиска категорий <see cref="CategoriesSearch"/>.
/// </summary>
public class CategoriesSearchValidator : AbstractValidator<CategoriesSearch>
{
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="CategoriesSearchValidator"/>.
    /// </summary>
    public CategoriesSearchValidator()
    {
        RuleFor(search => search.Skip)
            .GreaterThanOrEqualTo(0);
        RuleFor(search => search.Take)
            .GreaterThan(0);
        When(search => search.NameFilter != null, () =>
        {
            RuleFor(search => search.NameFilter!)
                .SetValidator(new TextFilterValidator());
        });
        RuleFor(search => search.Order)
            .NotNull();
        When(search => search.Order != null, () =>
        {
            RuleFor(search => search.Order!.By)
                .NotEqual(CategoriesOrderBy.None);
        });
    }
}