using System;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.AppServices.Exceptions.Users;

namespace ClassifiedsApi.AppServices.Contexts.Users.Validators;

/// <summary>
/// Валидатор прав пользователей.
/// </summary>
public interface IUserAccessValidator
{
    /// <summary>
    /// Валидирует права пользователя на объявление и вызывает исключение <see cref="AdvertAccessDeniedException"/>, если пользоваль не обладает ими.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="advertId">Идентификатор объявления.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns></returns>
    Task ValidateAdvertAccessAndThrowAsync(Guid userId, Guid advertId, CancellationToken token);
    
    /// <summary>
    /// Валидирует права пользователя на комментарий и вызывает исключение <see cref="CommentAccessDeniedException"/>, если пользоваль не обладает ими.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="commentId">Идентификатор комментария.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns></returns>
    Task ValidateCommentAccessAndThrowAsync(Guid userId, Guid commentId, CancellationToken token);
}