using ClassifiedsApi.Contracts.Contexts.Characteristics;
using FluentValidation;

namespace ClassifiedsApi.AppServices.Contexts.Characteristics.Validators;

/// <summary>
/// Валидатор модели добавления характеристики объявления <see cref="CharacteristicAdd"/>.
/// </summary>

public class CharacteristicAddValidator : AbstractValidator<CharacteristicAdd>
{
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="CharacteristicAddValidator"/>.
    /// </summary>
    public CharacteristicAddValidator()
    {
        RuleFor(characteristicAdd => characteristicAdd.Value)
            .NotNull()
            .SetValidator(new CharacteristicValueValidator());

        RuleFor(characteristicAdd => characteristicAdd.Name)
            .NotNull()
            .SetValidator(new CharacteristicNameValidator());
    }
}