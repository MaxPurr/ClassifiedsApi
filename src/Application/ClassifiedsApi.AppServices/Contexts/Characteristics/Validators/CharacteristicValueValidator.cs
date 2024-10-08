using ClassifiedsApi.AppServices.Common.Validators;
using FluentValidation;

namespace ClassifiedsApi.AppServices.Contexts.Characteristics.Validators;

/// <summary>
/// Валидатор значения характеристики.
/// </summary>

[IgnoreAutomaticRegistration]
public class CharacteristicValueValidator : AbstractValidator<string?>
{
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="CharacteristicValueValidator"/>.
    /// </summary>
    public CharacteristicValueValidator()
    {
        RuleFor(value => value)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MinimumLength(1)
            .MaximumLength(1000)
            .WithName("Value");
    }
}