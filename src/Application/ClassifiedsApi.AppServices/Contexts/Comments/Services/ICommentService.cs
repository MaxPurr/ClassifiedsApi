using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.Contracts.Contexts.Comments;

namespace ClassifiedsApi.AppServices.Contexts.Comments.Services;

/// <summary>
/// Сервис комментариев.
/// </summary>
public interface ICommentService
{
    /// <summary>
    /// Метод для создания комментария.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="advertId">Идентификатор объявления.</param>
    /// <param name="commentCreate">Модель создания комментария <see cref="CommentCreate"/>.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns>Идентификатор созданного комментария.</returns>
    public Task<Guid> CreateAsync(Guid userId, Guid advertId, CommentCreate commentCreate, CancellationToken token);
    
    /// <summary>
    /// Метод для получения информации о комментарии по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор комментария.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns>Модель информации о комментарии <see cref="CommentInfo"/>.</returns>
    public Task<CommentInfo> GetByIdAsync(Guid id, CancellationToken token);

    /// <summary>
    /// Метод поиска комментариев объявления по условию.
    /// </summary>
    /// <param name="advertId">Идентификатор объявления.</param>
    /// <param name="search">Модель поиска комментариев <see cref="CommentsSearch"/>.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns>Коллекция моделей информации о комментариях.</returns>
    public Task<IReadOnlyCollection<CommentInfo>> SearchAsync(Guid advertId, CommentsSearch search, CancellationToken token);
    
    /// <summary>
    /// Метод для удаления комментария.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="commentId">Идентификатор комментария.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns></returns>
    public Task DeleteAsync(Guid userId, Guid commentId, CancellationToken token);
    
    /// <summary>
    /// Метод для обновления комментария.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <param name="commentId">Идентификатор комментария.</param>
    /// <param name="commentUpdate">Модель обновления комментария <see cref="CommentUpdate"/>.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns>Модель обновленной информации о комментарии <see cref="CommentInfo"/>.</returns>
    public Task<CommentInfo> UpdateAsync(Guid userId, Guid commentId, CommentUpdate commentUpdate, CancellationToken token);
}