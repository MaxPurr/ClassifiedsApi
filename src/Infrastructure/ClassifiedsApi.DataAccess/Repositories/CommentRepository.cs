using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ClassifiedsApi.AppServices.Contexts.Comments.Repositories;
using ClassifiedsApi.AppServices.Exceptions.Comments;
using ClassifiedsApi.AppServices.Specifications;
using ClassifiedsApi.Contracts.Contexts.Comments;
using ClassifiedsApi.DataAccess.DbContexts;
using ClassifiedsApi.Domain.Entities;
using ClassifiedsApi.Infrastructure.Repository.Sql;
using Microsoft.EntityFrameworkCore;

namespace ClassifiedsApi.DataAccess.Repositories;

/// <inheritdoc/>
public class CommentRepository : ICommentRepository
{
    private readonly ISqlRepository<Comment, ApplicationDbContext> _repository;
    private readonly IMapper _mapper;
    
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="CommentRepository"/>.
    /// </summary>
    /// <param name="repository">Глуппый репозиторий <see cref="ISqlRepository{TEntity, TContext}"/>.</param>
    /// <param name="mapper">Маппер <see cref="IMapper"/>.</param>
    public CommentRepository(ISqlRepository<Comment, ApplicationDbContext> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    /// <inheritdoc/>
    public async Task<Guid> CreateAsync(CommentCreateRequest createRequest, CancellationToken token)
    {
        var comment = _mapper.Map<Comment>(createRequest);
        await _repository.AddAsync(comment, token);
        return comment.Id;
    }
    
    /// <inheritdoc/>
    public async Task<CommentInfo> GetByIdAsync(Guid id, CancellationToken token)
    {
        var comment = await _repository
            .GetByPredicate(comment => comment.Id == id)
            .ProjectTo<CommentInfo>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(token);
        if (comment == null)
        {
            throw new CommentNotFoundException();
        }
        return comment;
    }

    private static Expression<Func<CommentInfo, object?>> GetOrderByExpression(CommentsOrderBy orderBy)
    {
        return orderBy switch
        {
            CommentsOrderBy.CreatedAt => comment => comment.CreatedAt,
            _ => comment => comment.Id,
        };
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyCollection<CommentInfo>> GetBySpecificationWithPaginationAsync(
        ISpecification<CommentInfo> specification, 
        int? skip, 
        int take, 
        CommentsOrder order,
        CancellationToken token)
    {
        var query = _repository
            .GetAll()
            .Include(comment => comment.User)
            .ProjectTo<CommentInfo>(_mapper.ConfigurationProvider)
            .Where(specification.PredicateExpression);
        var orderByExpression = GetOrderByExpression(order.By);
        query = order.Descending 
            ? query.OrderByDescending(orderByExpression)
            : query.OrderBy(orderByExpression);
        if (skip.HasValue)
        {
            query = query.Skip(skip.Value);
        }
        return await query
            .Take(take)
            .ToArrayAsync(token);
    }
    
    /// <inheritdoc/>
    public async Task DeleteAsync(Guid id, DateTime deletedAt, CancellationToken token)
    {
        var comment = await _repository.FirstOrDefaultAsync(comment => comment.Id == id, token);
        if (comment == null)
        {
            throw new CommentNotFoundException();
        }
        comment.Text = string.Empty;
        comment.DeletedAt = deletedAt;
        await _repository.UpdateAsync(comment, token);
    }

    /// <inheritdoc/>
    public async Task<CommentInfo> UpdateAsync(
        Guid id, 
        CommentUpdate commentUpdate, 
        DateTime updatedAt,
        CancellationToken token)
    {
        var comment = await _repository.FirstOrDefaultAsync(comment => comment.Id == id, token);
        if (comment == null)
        {
            throw new CommentNotFoundException();
        }
        if (commentUpdate.Text != null)
        {
            comment.Text = commentUpdate.Text;
        }
        comment.UpdatedAt = updatedAt;
        await _repository.UpdateAsync(comment, token);
        return _mapper.Map<CommentInfo>(comment);
    }

    /// <inheritdoc/>
    public Task<bool> IsExistsAsync(Guid id, CancellationToken token)
    {
        return _repository.IsAnyExistAsync(comment => comment.Id == id, token);
    }
    
    /// <inheritdoc/>
    public Task<bool> IsExistsAsync(Guid advertId, Guid commentId, CancellationToken token)
    {
        return _repository.IsAnyExistAsync(comment => 
            comment.AdvertId == advertId &&
            comment.Id == commentId, token);
    }
    
    /// <inheritdoc/>
    public async Task<Guid> GetUserIdAsync(Guid id, CancellationToken token)
    {
        var userId = await _repository
            .GetByPredicate(comment => comment.Id == id)
            .Select<Comment, Guid?>(comment => comment.UserId)
            .FirstOrDefaultAsync(token);
        if (!userId.HasValue)
        {
            throw new CommentNotFoundException();
        }
        return userId.Value;
    }

    /// <inheritdoc/>
    public Task<DateTime?> GetDeletedAtAsync(Guid id, CancellationToken token)
    {
        return _repository
            .GetByPredicate(comment => comment.Id == id)
            .Select(comment => comment.DeletedAt)
            .FirstOrDefaultAsync(token);
    }
}