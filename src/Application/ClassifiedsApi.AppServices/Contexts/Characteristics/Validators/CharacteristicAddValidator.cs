using System;
using ClassifiedsApi.AppServices.Common.Validators;
using ClassifiedsApi.AppServices.Contexts.Characteristics.Repositories;
using ClassifiedsApi.Contracts.Contexts.Characteristics;
using FluentValidation;

namespace ClassifiedsApi.AppServices.Contexts.Characteristics.Validators;

/// <summary>
/// Валидатор модели добавления характеристики объявления.
/// </summary>

[IgnoreAutomaticRegistration]
public class CharacteristicAddValidator : AbstractValidator<CharacteristicAdd>
{
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="CharacteristicAddValidator"/>.
    /// </summary>
    /// <param name="advertId">Идентификатор объявления.</param>
    /// <param name="characteristicRepository">Репозиторий характеристик объявлений <see cref="ICharacteristicRepository"/>.</param>
    public CharacteristicAddValidator(Guid advertId, ICharacteristicRepository characteristicRepository)
    {
        RuleFor(characteristicAdd => characteristicAdd.Value)
            .SetValidator(new CharacteristicValueValidator());

        RuleFor(characteristicAdd => characteristicAdd.Name)
            .SetValidator(new CharacteristicNameValidator(advertId, characteristicRepository));
    }
}