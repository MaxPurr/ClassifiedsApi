using ClassifiedsApi.AppServices.Contexts.Adverts.Repositories;
using ClassifiedsApi.AppServices.Contexts.Adverts.Validators;
using ClassifiedsApi.AppServices.Contexts.Characteristics.Repositories;
using ClassifiedsApi.Contracts.Contexts.Characteristics;
using FluentValidation;

namespace ClassifiedsApi.AppServices.Contexts.Characteristics.Validators;

/// <summary>
/// Валидатор модели пользовательского запроса на обновление характеристики объявления.
/// </summary>
public class CharacteristicUpdateRequestValidator : UserAdvertRequestValidator<CharacteristicUpdateRequest>
{
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="CharacteristicUpdateRequestValidator"/>.
    /// </summary>
    /// <param name="advertRepository">Репозиторий объявлений <see cref="IAdvertRepository"/>.</param>
    /// <param name="characteristicRepository">Репозиторий характеристик объявлений <see cref="ICharacteristicRepository"/>.</param>
    public CharacteristicUpdateRequestValidator(
        IAdvertRepository advertRepository,
        ICharacteristicRepository characteristicRepository) 
        : base(advertRepository)
    {
        RuleFor(updateRequest => updateRequest.Model)
            .Must(IsNotEmpty)
            .WithMessage("Модель обновления характеристики объявления не может быть пустой.")
            .SetValidator(updateRequest => new CharacteristicUpdateValidator(updateRequest.AdvertId, characteristicRepository));
    }

    private bool IsNotEmpty(CharacteristicUpdate characteristicUpdate)
    {
        return characteristicUpdate.Name != null
               || characteristicUpdate.Value != null;
    }
}