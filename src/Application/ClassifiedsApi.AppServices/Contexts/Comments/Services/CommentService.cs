using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.AppServices.Contexts.Adverts.Services;
using ClassifiedsApi.AppServices.Contexts.Comments.Builders;
using ClassifiedsApi.AppServices.Contexts.Comments.Repositories;
using ClassifiedsApi.AppServices.Contexts.Users.Services;
using ClassifiedsApi.Contracts.Contexts.Comments;
using FluentValidation;

namespace ClassifiedsApi.AppServices.Contexts.Comments.Services;

/// <inheritdoc />
public class CommentService : ICommentService
{
    private readonly ICommentRepository _repository;
    private readonly ICommentSpecificationBuilder _specificationBuilder;
    private readonly IUserAccessVerifier _userAccessVerifier;
    private readonly IAdvertVerifier _advertVerifier;
    private readonly ICommentVerifier _commentVerifier;
    private readonly TimeProvider _timeProvider;
    
    private readonly IValidator<CommentCreate> _createValidator;
    private readonly IValidator<CommentsSearch> _searchValidator;
    private readonly IValidator<CommentUpdate> _updateValidator;

    /// <summary>
    /// Инициализирует экземпляр класса <see cref="CommentService"/>.
    /// </summary>
    /// <param name="repository">Репозиторий комментариев <see cref="ICommentRepository"/>.</param>
    /// <param name="specificationBuilder">Строитель спецификаций для комментариев <see cref="ICommentSpecificationBuilder"/>.</param>
    /// <param name="userAccessVerifier">Верификатор прав пользователя <see cref="IUserAccessVerifier"/>.</param>
    /// <param name="advertVerifier">Верификатор объявлений <see cref="IAdvertVerifier"/>.</param>
    /// <param name="commentVerifier">Верификатор комментариев <see cref="ICommentVerifier"/>.</param>
    /// <param name="timeProvider">Провайдер времени <see cref="TimeProvider"/>.</param>
    /// <param name="searchValidator">Валидатор модели поиска комментариев.</param>
    /// <param name="createValidator">Валидатор модели создания комментария.</param>
    /// <param name="updateValidator">Валидатор модели обновления комментария.</param>
    public CommentService(
        ICommentRepository repository, 
        ICommentSpecificationBuilder specificationBuilder, 
        IUserAccessVerifier userAccessVerifier,
        IAdvertVerifier advertVerifier,
        ICommentVerifier commentVerifier,
        TimeProvider timeProvider,
        IValidator<CommentsSearch> searchValidator, 
        IValidator<CommentCreate> createValidator, 
        IValidator<CommentUpdate> updateValidator)
    {
        _repository = repository;
        _specificationBuilder = specificationBuilder;
        _searchValidator = searchValidator;
        _createValidator = createValidator;
        _timeProvider = timeProvider;
        _updateValidator = updateValidator;
        _commentVerifier = commentVerifier;
        _advertVerifier = advertVerifier;
        _userAccessVerifier = userAccessVerifier;
    }
    
    /// <inheritdoc />
    public async Task<Guid> CreateAsync(Guid userId, Guid advertId, CommentCreate commentCreate, CancellationToken token)
    {
        _createValidator.ValidateAndThrow(commentCreate);
        await _advertVerifier.VerifyExistsAndThrowAsync(advertId, token);
        if (commentCreate.ParentId.HasValue)
        {
            await _commentVerifier.VerifyExistsAndThrowAsync(advertId, commentCreate.ParentId.Value, token);
        }
        var createRequest = new CommentCreateRequest
        {
            UserId = userId,
            AdvertId = advertId,
            CommentCreate = commentCreate
        };
        return await _repository.CreateAsync(createRequest, token);
    }
    
    /// <inheritdoc />
    public Task<CommentInfo> GetByIdAsync(Guid id, CancellationToken token)
    {
        return _repository.GetByIdAsync(id, token);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyCollection<CommentInfo>> SearchAsync(Guid advertId, CommentsSearch search, CancellationToken token)
    {
        await _advertVerifier.VerifyExistsAndThrowAsync(advertId, token);
        _searchValidator.ValidateAndThrow(search);
        var specification = _specificationBuilder.Build(advertId, search);
        var comments = await _repository.GetBySpecificationWithPaginationAsync(
            specification: specification,
            skip: search.Skip,
            take: search.Take.GetValueOrDefault(),
            order: search.Order!,
            token: token);
        return comments;
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid userId, Guid commentId, CancellationToken token)
    {
        await _userAccessVerifier.VerifyCommentAccessAndThrowAsync(userId, commentId, token);
        await _commentVerifier.VerifyIsNotDeletedAndThrowAsync(commentId, token);
        var deletedAt = _timeProvider.GetUtcNow().UtcDateTime;
        await _repository.DeleteAsync(commentId, deletedAt, token);
    }

    /// <inheritdoc />
    public async Task<CommentInfo> UpdateAsync(Guid userId, Guid commentId, CommentUpdate commentUpdate, CancellationToken token)
    {
        await _userAccessVerifier.VerifyCommentAccessAndThrowAsync(userId, commentId, token);
        await _commentVerifier.VerifyIsNotDeletedAndThrowAsync(commentId, token);
        _updateValidator.ValidateAndThrow(commentUpdate);
        var updatedAt = _timeProvider.GetUtcNow().UtcDateTime;
        var updatedComment = await _repository.UpdateAsync(commentId, commentUpdate, updatedAt, token);
        return updatedComment;
    }
}