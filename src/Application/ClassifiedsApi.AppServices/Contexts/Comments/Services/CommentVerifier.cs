using System;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.AppServices.Contexts.Comments.Repositories;
using ClassifiedsApi.AppServices.Exceptions.Comments;

namespace ClassifiedsApi.AppServices.Contexts.Comments.Services;

/// <inheritdoc />
public class CommentVerifier : ICommentVerifier
{
    private readonly ICommentRepository _repository;
    
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="CommentVerifier"/>.
    /// </summary>
    /// <param name="repository">Репозиторий комментариев <see cref="ICommentRepository"/>.</param>
    public CommentVerifier(ICommentRepository repository)
    {
        _repository = repository;
    }

    /// <inheritdoc />
    public async Task VerifyExistsAndThrowAsync(Guid advertId, Guid commentId, CancellationToken token)
    {
        var exists = await _repository.IsExistsAsync(advertId, commentId, token);
        if (!exists)
        {
            throw new AdvertCommentNotExistsException();
        }
    }
    
    /// <inheritdoc />
    public async Task VerifyIsNotDeletedAndThrowAsync(Guid commentId, CancellationToken token)
    {
        var deletedAt = await _repository.GetDeletedAtAsync(commentId, token);
        if (deletedAt.HasValue)
        {
            throw new CommentHasBeenDeletedException();
        }
    }
}