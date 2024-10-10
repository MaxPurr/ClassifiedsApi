using ClassifiedsApi.AppServices.Contexts.Adverts.Repositories;
using ClassifiedsApi.AppServices.Contexts.Adverts.Validators;
using ClassifiedsApi.AppServices.Contexts.Characteristics.Repositories;
using ClassifiedsApi.Contracts.Contexts.Characteristics;
using FluentValidation;

namespace ClassifiedsApi.AppServices.Contexts.Characteristics.Validators;

/// <summary>
/// Валидатор модели пользовательского запроса на обновление характеристики объявления <see cref="CharacteristicUpdateRequest"/>.
/// </summary>
public class CharacteristicUpdateRequestValidator : AdvertRequestValidator<CharacteristicUpdateRequest>
{
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="CharacteristicUpdateRequestValidator"/>.
    /// </summary>
    /// <param name="advertRepository">Репозиторий характеристик объявлений <see cref="IAdvertRepository"/>.</param>
    /// <param name="characteristicRepository">Репозиторий характеристик объявлений <see cref="ICharacteristicRepository"/>.</param>
    public CharacteristicUpdateRequestValidator(
        IAdvertRepository advertRepository,
        ICharacteristicRepository characteristicRepository) 
        : base(advertRepository)
    {
        RuleFor(request => request.Model)
            .Must(IsNotEmpty)
            .WithMessage("Модель обновления характеристики объявления не может быть пустой.")
            .SetValidator(request => new CharacteristicUpdateValidator(request.AdvertId, characteristicRepository));
    }

    private static bool IsNotEmpty(CharacteristicUpdate characteristicUpdate)
    {
        return characteristicUpdate.Name != null || 
               characteristicUpdate.Value != null;
    }
}