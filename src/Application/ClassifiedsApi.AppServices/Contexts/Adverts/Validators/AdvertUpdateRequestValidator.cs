using ClassifiedsApi.AppServices.Contexts.Adverts.Repositories;
using ClassifiedsApi.AppServices.Contexts.Categories.Repositories;
using ClassifiedsApi.Contracts.Contexts.Adverts;
using FluentValidation;

namespace ClassifiedsApi.AppServices.Contexts.Adverts.Validators;

/// <summary>
/// Валидатор модели пользовательского запроса на обновление объявления.
/// </summary>
public class AdvertUpdateRequestValidator : AdvertRequestValidator<AdvertRequest<AdvertUpdate>>
{
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="AdvertUpdateRequestValidator"/>.
    /// </summary>
    /// <param name="advertRepository">Репозиторий объявлений <see cref="IAdvertRepository"/>.</param>
    /// <param name="categoryRepository">Репозиторий категорий <see cref="ICategoryRepository"/>.</param>
    public AdvertUpdateRequestValidator(IAdvertRepository advertRepository, ICategoryRepository categoryRepository) : base(advertRepository)
    {
        RuleFor(update => update.Model)
            .Cascade(CascadeMode.Stop)
            .Must(IsNotEmpty)
            .WithMessage("Модель обновления объявления не может быть пустой.")
            .SetValidator(new AdvertUpdateValidator(categoryRepository));
    }

    private static bool IsNotEmpty(AdvertUpdate advertUpdate)
    {
        return advertUpdate.Title != null 
            || advertUpdate.Description != null
            || advertUpdate.CategoryId != null
            || advertUpdate.Price != null
            || advertUpdate.Disabled != null;
    }
}