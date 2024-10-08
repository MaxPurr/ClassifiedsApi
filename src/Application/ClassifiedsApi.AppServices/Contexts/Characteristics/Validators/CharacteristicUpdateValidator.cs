using System;
using ClassifiedsApi.AppServices.Common.Validators;
using ClassifiedsApi.AppServices.Contexts.Characteristics.Repositories;
using ClassifiedsApi.Contracts.Contexts.Characteristics;
using FluentValidation;

namespace ClassifiedsApi.AppServices.Contexts.Characteristics.Validators;

/// <summary>
/// Валидатор модели обновления характеристики объявления <see cref="CharacteristicUpdate"/>.
/// </summary>

[IgnoreAutomaticRegistration]
public class CharacteristicUpdateValidator : AbstractValidator<CharacteristicUpdate>
{
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="CharacteristicUpdateValidator"/>.
    /// </summary>
    /// <param name="advertId">Идентификатор объявления.</param>
    /// <param name="characteristicRepository">Репозиторий характеристик объявлений <see cref="ICharacteristicRepository"/>.</param>
    public CharacteristicUpdateValidator(Guid advertId, ICharacteristicRepository characteristicRepository)
    {
        When(characteristicUpdate => characteristicUpdate.Name != null, () =>
        {
            RuleFor(characteristicUpdate => characteristicUpdate.Name)
                .SetValidator(new CharacteristicNameValidator(advertId, characteristicRepository));
        });
        
        When(characteristicUpdate => characteristicUpdate.Value != null, () =>
        {
            RuleFor(characteristicUpdate => characteristicUpdate.Value)
                .SetValidator(new CharacteristicValueValidator());
        });
    }
}