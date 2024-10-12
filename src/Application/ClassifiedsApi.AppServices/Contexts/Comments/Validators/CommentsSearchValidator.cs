using System;
using ClassifiedsApi.AppServices.Common.Validators;
using ClassifiedsApi.Contracts.Contexts.Comments;
using FluentValidation;

namespace ClassifiedsApi.AppServices.Contexts.Comments.Validators;

/// <summary>
/// Валидатор модели поиска комментариев. 
/// </summary>
public class CommentsSearchValidator : BasePaginationValidator<CommentsSearch>
{
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="CommentsSearchValidator"/>.
    /// </summary>
    public CommentsSearchValidator()
    {
        When(search => search.FilterByParentId != null &&
                       search.FilterByParentId!.ParentId != null, () =>
        {
            RuleFor(search => search.FilterByParentId!.ParentId)
                .NotEqual(Guid.Empty);
        });

        RuleFor(search => search.Order)
            .NotNull();
        
        When(search => search.Order != null, () =>
        {
            RuleFor(search => search.Order!.By)
                .NotEqual(CommentsOrderBy.None);
        });
    }
}