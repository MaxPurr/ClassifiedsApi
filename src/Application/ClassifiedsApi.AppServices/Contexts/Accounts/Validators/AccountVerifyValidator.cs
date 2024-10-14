using ClassifiedsApi.Contracts.Contexts.Accounts;
using FluentValidation;

namespace ClassifiedsApi.AppServices.Contexts.Accounts.Validators;

/// <summary>
/// Валидатор модели для проверки учетных данных аккаунта.
/// </summary>
public class AccountVerifyValidator : AbstractValidator<AccountVerify>
{
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="AccountRegisterValidator"/>.
    /// </summary>
    public AccountVerifyValidator()
    {
        RuleFor(verify => verify.Login)
            .NotNull()
            .MinimumLength(1)
            .MaximumLength(255);
        
        RuleFor(verify => verify.Password)
            .NotNull()
            .MinimumLength(8)
            .MaximumLength(128);
    }
}