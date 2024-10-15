using System;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.AppServices.Contexts.Comments.Repositories;
using ClassifiedsApi.AppServices.Exceptions.Comments;

namespace ClassifiedsApi.AppServices.Contexts.Comments.Validators;

/// <inheritdoc />
public class CommentValidator : ICommentValidator
{
    private readonly ICommentRepository _repository;
    
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="CommentValidator"/>.
    /// </summary>
    /// <param name="repository">Репозиторий комментариев <see cref="ICommentRepository"/>.</param>
    public CommentValidator(ICommentRepository repository)
    {
        _repository = repository;
    }

    /// <inheritdoc />
    public Task<bool> ValidateExistsAsync(Guid advertId, Guid commentId, CancellationToken token)
    {
        return _repository.IsExistsAsync(advertId, commentId, token);
    }
    
    /// <inheritdoc />
    public async Task ValidateIsNotDeletedAndThrowAsync(Guid commentId, CancellationToken token)
    {
        var deletedAt = await _repository.GetDeletedAtAsync(commentId, token);
        if (deletedAt.HasValue)
        {
            throw new CommentHasBeenDeletedException();
        }
    }
}