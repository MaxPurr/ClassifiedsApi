using System;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.AppServices.Exceptions.Comments;

namespace ClassifiedsApi.AppServices.Contexts.Comments.Services;

/// <summary>
/// Верификатор комментариев.
/// </summary>
public interface ICommentVerifier
{
    /// <summary>
    /// Проверяет, что у объявления есть комментарий с указанным идентификатором и вызывает исключение <see cref="AdvertCommentNotExistsException"/> если нет.
    /// </summary>
    /// <param name="advertId">Идентификатор объявления.</param>
    /// <param name="commentId">Идентификатор комментария.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns></returns>
    Task VerifyExistsAndThrowAsync(Guid advertId, Guid commentId, CancellationToken token);
    
    /// <summary>
    /// Проверяет, что комментарий не был удален и вызывает исключение <see cref="CommentHasBeenDeletedException"/> если был.
    /// </summary>
    /// <param name="commentId">Идентификатор комментария.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns></returns>
    Task VerifyIsNotDeletedAndThrowAsync(Guid commentId, CancellationToken token);
}