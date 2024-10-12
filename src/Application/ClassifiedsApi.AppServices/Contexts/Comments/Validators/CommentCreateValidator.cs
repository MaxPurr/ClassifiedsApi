using System;
using ClassifiedsApi.Contracts.Contexts.Comments;
using FluentValidation;

namespace ClassifiedsApi.AppServices.Contexts.Comments.Validators;

/// <summary>
/// Валидатор модели создания комментария <see cref="CommentCreate"/>.
/// </summary>
public class CommentCreateValidator : AbstractValidator<CommentCreate>
{
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="CommentCreateValidator"/>.
    /// </summary>
    public CommentCreateValidator()
    {
        RuleFor(create => create.Text)
            .NotNull()
            .MinimumLength(1)
            .MaximumLength(1000);

        When(create => create.ParentId != null, () =>
        {
            RuleFor(create => create.ParentId)
                .NotEqual(Guid.Empty);
        });
    }
}