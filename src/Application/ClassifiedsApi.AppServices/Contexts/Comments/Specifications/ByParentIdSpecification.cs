using System;
using System.Linq.Expressions;
using ClassifiedsApi.AppServices.Specifications;
using ClassifiedsApi.Contracts.Contexts.Comments;

namespace ClassifiedsApi.AppServices.Contexts.Comments.Specifications;

/// <summary>
/// Спецификация для фильтрации комментариев по идентификатору родительского комментария.
/// </summary>
public class ByParentIdSpecification : Specification<CommentInfo>
{
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="ByParentIdSpecification"/>.
    /// </summary>
    /// <param name="parentId">Идентификатор родительского комментария.</param>
    public ByParentIdSpecification(Guid? parentId)
    {
        PredicateExpression = comment => comment.ParentId == parentId;
    }
    
    /// <inheritdoc />
    public override Expression<Func<CommentInfo, bool>> PredicateExpression { get; }
}