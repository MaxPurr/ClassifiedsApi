using System;
using System.Linq.Expressions;
using ClassifiedsApi.AppServices.Specifications;
using ClassifiedsApi.Contracts.Contexts.Comments;

namespace ClassifiedsApi.AppServices.Contexts.Comments.Specifications;

/// <summary>
/// Спецификация для фильтрации комментариев по идентификатору объявления.
/// </summary>
public class ByAdvertIdSpecification : Specification<CommentInfo>
{
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="ByAdvertIdSpecification"/>.
    /// </summary>
    /// <param name="advertId">Идентификатор объявления.</param>
    public ByAdvertIdSpecification(Guid advertId)
    {
        PredicateExpression = comment => comment.AdvertId == advertId;
    }
    
    /// <inheritdoc />
    public override Expression<Func<CommentInfo, bool>> PredicateExpression { get; }
}