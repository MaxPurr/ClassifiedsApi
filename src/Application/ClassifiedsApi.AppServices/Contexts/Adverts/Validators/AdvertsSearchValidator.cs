using System;
using ClassifiedsApi.AppServices.Common.Validators;
using ClassifiedsApi.AppServices.Contexts.Categories.Repositories;
using ClassifiedsApi.Contracts.Contexts.Adverts;
using FluentValidation;

namespace ClassifiedsApi.AppServices.Contexts.Adverts.Validators;

/// <summary>
/// Валидатор модели поиска объявлений.
/// </summary>
public class AdvertsSearchValidator : BasePaginationValidator<AdvertsSearch>
{
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="AdvertsSearchValidator"/>.
    /// </summary>
    /// <param name="categoryRepository">Репозиторий категорий <see cref="ICategoryRepository"/>.</param>
    public AdvertsSearchValidator(ICategoryRepository categoryRepository)
    {
        When(search => search.TextFilter != null, () =>
        {
            RuleFor(search => search.TextFilter!)
                .SetValidator(new TextFilterValidator());
        });

        RuleFor(search => search.MinPrice)
            .GreaterThanOrEqualTo(0);
        
        RuleFor(search => search.MaxPrice)
            .GreaterThanOrEqualTo(0);

        When(search => search.FilterByCategoryId != null, () =>
        {
            RuleFor(search => search.FilterByCategoryId)
                .NotEqual(Guid.Empty);
        }); 
        
        RuleFor(search => search.Order)
            .NotNull();
        
        When(search => search.Order != null, () =>
        {
            RuleFor(search => search.Order!.By)
                .NotEqual(AdvertsOrderBy.None);
        });
    }
}