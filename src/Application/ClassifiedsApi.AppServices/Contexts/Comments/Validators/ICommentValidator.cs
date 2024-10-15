using System;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.AppServices.Exceptions.Comments;

namespace ClassifiedsApi.AppServices.Contexts.Comments.Validators;

/// <summary>
/// Валидатор комментариев.
/// </summary>
public interface ICommentValidator
{
    /// <summary>
    /// Проверяет, что у объявления есть комментарий с указанным идентификатором.
    /// </summary>
    /// <param name="advertId">Идентификатор объявления.</param>
    /// <param name="commentId">Идентификатор комментария.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns><code data-dev-comment-type="langword">true</code> если комментарий найден, иначе <code data-dev-comment-type="langword">false</code>.</returns>
    Task<bool> ValidateExistsAsync(Guid advertId, Guid commentId, CancellationToken token);
    
    /// <summary>
    /// Проверяет, что комментарий не был удален и вызывает исключение <see cref="CommentHasBeenDeletedException"/> если был.
    /// </summary>
    /// <param name="commentId">Идентификатор комментария.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns></returns>
    Task ValidateIsNotDeletedAndThrowAsync(Guid commentId, CancellationToken token);
}