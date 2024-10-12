using ClassifiedsApi.AppServices.Common.Validators;
using FluentValidation;

namespace ClassifiedsApi.AppServices.Contexts.Characteristics.Validators;

/// <summary>
/// Валидатор названия характеристики.
/// </summary>

[IgnoreAutomaticRegistration]
public class CharacteristicNameValidator : AbstractValidator<string?>
{
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="CharacteristicNameValidator"/>.
    /// </summary>
    public CharacteristicNameValidator()
    {
        RuleFor(name => name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(255)
            .WithName("Name");
    }
}