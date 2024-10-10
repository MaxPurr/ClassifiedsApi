using ClassifiedsApi.AppServices.Contexts.Adverts.Repositories;
using ClassifiedsApi.AppServices.Contexts.Adverts.Validators;
using ClassifiedsApi.AppServices.Contexts.Characteristics.Repositories;
using ClassifiedsApi.Contracts.Contexts.Adverts;
using ClassifiedsApi.Contracts.Contexts.Characteristics;

namespace ClassifiedsApi.AppServices.Contexts.Characteristics.Validators;

/// <summary>
/// Валидатор модели пользовательского запроса на добавление характеристики объявления.
/// </summary>
public class CharacteristicAddRequestValidator : AdvertRequestValidator<AdvertRequest<CharacteristicAdd>>
{
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="CharacteristicAddRequestValidator"/>.
    /// </summary>
    /// <param name="advertRepository">Репозиторий объявлений <see cref="IAdvertRepository"/>.</param>
    /// <param name="characteristicRepository">Репозиторий характеристик объявлений <see cref="ICharacteristicRepository"/>.</param>
    public CharacteristicAddRequestValidator(
        IAdvertRepository advertRepository,
        ICharacteristicRepository characteristicRepository) 
        : base(advertRepository)
    {
        RuleFor(addRequest => addRequest.Model)
            .SetValidator(addRequest => new CharacteristicAddValidator(addRequest.AdvertId, characteristicRepository));
    }
}