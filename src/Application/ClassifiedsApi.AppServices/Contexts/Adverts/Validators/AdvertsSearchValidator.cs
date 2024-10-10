using System;
using ClassifiedsApi.AppServices.Common.Validators;
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
    public AdvertsSearchValidator()
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

        RuleFor(search => search.FilterByCategoryId)
            .NotEqual(Guid.Empty);
        
        RuleFor(search => search.Order)
            .NotNull();
        
        When(search => search.Order != null, () =>
        {
            RuleFor(search => search.Order!.By)
                .NotEqual(AdvertsOrderBy.None);
        });
    }
}