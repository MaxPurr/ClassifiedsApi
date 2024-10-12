using ClassifiedsApi.Contracts.Contexts.Comments;
using FluentValidation;

namespace ClassifiedsApi.AppServices.Contexts.Comments.Validators;

/// <summary>
/// Валидатор модели обновления комментария.
/// </summary>
public class CommentUpdateValidator : AbstractValidator<CommentUpdate>
{
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="CommentUpdateValidator"/>.
    /// </summary>
    public CommentUpdateValidator()
    {
        RuleFor(update => update.Text)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .MinimumLength(1)
            .MaximumLength(1000);
    }
}