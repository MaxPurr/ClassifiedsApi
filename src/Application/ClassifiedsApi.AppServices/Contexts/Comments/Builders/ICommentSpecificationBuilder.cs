using System;
using ClassifiedsApi.AppServices.Specifications;
using ClassifiedsApi.Contracts.Contexts.Comments;

namespace ClassifiedsApi.AppServices.Contexts.Comments.Builders;

/// <summary>
/// Строитель спецификаций для комментариев.
/// </summary>
public interface ICommentSpecificationBuilder
{
    /// <summary>
    /// Строит спецификацию по идентификатору объявления.
    /// </summary>
    /// <param name="advertId">Идентификатор объявления.</param>
    /// <param name="search">Модель поиска комментариев <see cref="CommentsSearch"/>.</param>
    /// <returns>Спецификация.</returns>
    ISpecification<CommentInfo> Build(Guid advertId, CommentsSearch search);
}