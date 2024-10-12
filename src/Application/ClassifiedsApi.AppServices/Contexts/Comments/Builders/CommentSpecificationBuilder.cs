using System;
using ClassifiedsApi.AppServices.Contexts.Comments.Specifications;
using ClassifiedsApi.AppServices.Specifications;
using ClassifiedsApi.Contracts.Contexts.Comments;

namespace ClassifiedsApi.AppServices.Contexts.Comments.Builders;

/// <inheritdoc />
public class CommentSpecificationBuilder : ICommentSpecificationBuilder
{
    /// <inheritdoc />
    public ISpecification<CommentInfo> Build(Guid advertId, CommentsSearch search)
    {
        Specification<CommentInfo> specification = new ByAdvertIdSpecification(advertId);
        if (search.FilterByParentId != null)
        {
            specification &= new ByParentIdSpecification(search.FilterByParentId.ParentId);
        }
        return specification;
    }
}