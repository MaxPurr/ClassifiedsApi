using ClassifiedsApi.Contracts.Common;
using FluentValidation;

namespace ClassifiedsApi.AppServices.Common.Validators;

/// <summary>
/// Валидатор модели поиска строки в тексте <see cref="TextFilter"/>.
/// </summary>

[IgnoreAutomaticRegistration]
public class TextFilterValidator : AbstractValidator<TextFilter>
{
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="TextFilterValidator"/>.
    /// </summary>
    public TextFilterValidator()
    {
        RuleFor(filter => filter.Query)
            .NotEmpty();
        RuleFor(filter => filter.QueryPosition)
            .NotEqual(QueryPosition.None);
    }
}