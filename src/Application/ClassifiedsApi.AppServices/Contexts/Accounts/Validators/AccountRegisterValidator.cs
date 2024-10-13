using ClassifiedsApi.Contracts.Contexts.Accounts;
using FluentValidation;

namespace ClassifiedsApi.AppServices.Contexts.Accounts.Validators;

/// <summary>
/// Валидатор модели регистрации нового аккаунта.
/// </summary>
public class AccountRegisterValidator : AbstractValidator<AccountRegister>
{
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="AccountRegisterValidator"/>.
    /// </summary>
    public AccountRegisterValidator()
    {
        RuleFor(register => register.Login)
            .NotNull()
            .MinimumLength(1)
            .MaximumLength(255);
        
        RuleFor(register => register.Password)
            .NotNull()
            .MinimumLength(8)
            .MaximumLength(128);
        
        RuleFor(register => register.FirstName)
            .NotNull()
            .MinimumLength(1)
            .MaximumLength(255);
        
        RuleFor(register => register.LastName)
            .NotNull()
            .MinimumLength(1)
            .MaximumLength(255);

        RuleFor(register => register.Email)
            .NotNull()
            .EmailAddress();

        When(register => register.Phone != null, () =>
        {
            RuleFor(register => register.Phone)
                .Matches(@"^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7,10}$")
                .WithName("Phone")
                .WithMessage("'{PropertyName}' неправильный формат номера телефона.");
        });

        RuleFor(register => register.BirthDate)
            .NotNull();
    }
}