using ClassifiedsApi.Contracts.Contexts.Characteristics;
using FluentValidation;

namespace ClassifiedsApi.AppServices.Contexts.Characteristics.Validators;

/// <summary>
/// Валидатор модели обновления характеристики объявления <see cref="CharacteristicUpdate"/>.
/// </summary>

public class CharacteristicUpdateValidator : AbstractValidator<CharacteristicUpdate>
{
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="CharacteristicUpdateValidator"/>.
    /// </summary>
    public CharacteristicUpdateValidator()
    {
        RuleFor(characteristicUpdate => characteristicUpdate)
            .Must(IsNotEmpty)
            .WithMessage("Модель обновления характеристики объявления не может быть пустой.");
        
        When(characteristicUpdate => characteristicUpdate.Name != null, () =>
        {
            RuleFor(characteristicUpdate => characteristicUpdate.Name)
                .SetValidator(new CharacteristicNameValidator());
        });
        
        When(characteristicUpdate => characteristicUpdate.Value != null, () =>
        {
            RuleFor(characteristicUpdate => characteristicUpdate.Value)
                .SetValidator(new CharacteristicValueValidator());
        });
    }
    
    private static bool IsNotEmpty(CharacteristicUpdate characteristicUpdate)
    {
        return characteristicUpdate.Name != null || 
               characteristicUpdate.Value != null;
    }
}