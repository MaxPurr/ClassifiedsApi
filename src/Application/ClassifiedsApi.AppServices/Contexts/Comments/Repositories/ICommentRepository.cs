using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.AppServices.Specifications;
using ClassifiedsApi.Contracts.Contexts.Comments;

namespace ClassifiedsApi.AppServices.Contexts.Comments.Repositories;

/// <summary>
/// Репозиторий комментариев.
/// </summary>
public interface ICommentRepository
{
    /// <summary>
    /// Метод для создания комментария.
    /// </summary>
    /// <param name="createRequest">Запрос на создание комментария <see cref="CommentCreateRequest"/>.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns>Идентификатор созданного комментария.</returns>
    Task<Guid> CreateAsync(CommentCreateRequest createRequest, CancellationToken token);
    
    /// <summary>
    /// Метод для получения информации о комментарии по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор комментария.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns>Модель информации о комментарии <see cref="CommentInfo"/>.</returns>
    Task<CommentInfo> GetByIdAsync(Guid id, CancellationToken token);
    
    /// <summary>
    /// Метод для получения комментариев объявления с пагинацией.
    /// </summary>
    /// <param name="specification">Спецификация <see cref="ISpecification{TEntity}"/>.</param>
    /// <param name="skip">Количество элементов для пропуска.</param>
    /// <param name="take">Количество элементов для получения.</param>
    /// <param name="order">Модель сортировки комментариев <see cref="CommentsOrder"/>.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns>Коллекция моделей информации о комментариях.</returns>
    Task<IReadOnlyCollection<CommentInfo>> GetBySpecificationWithPaginationAsync(
        ISpecification<CommentInfo> specification, 
        int? skip, 
        int take, 
        CommentsOrder order, 
        CancellationToken token);
    
    /// <summary>
    /// Метод для удаления комментария.
    /// </summary>
    /// <param name="id">Идентификатор комментария.</param>
    /// <param name="deletedAt">Дата и время удаления.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns></returns>
    Task DeleteAsync(Guid id,DateTime deletedAt, CancellationToken token);

    /// <summary>
    /// Метод для обновления комментария.
    /// </summary>
    /// <param name="id">Идентификатор комментария.</param>
    /// <param name="commentUpdate">Модель обновления комментария <see cref="CommentUpdate"/>.</param>
    /// <param name="updatedAt">Дата и время обновления.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns>Модель обновленной информации о комментарии <see cref="CommentInfo"/>.</returns>
    Task<CommentInfo> UpdateAsync(
        Guid id, 
        CommentUpdate commentUpdate,
        DateTime updatedAt,
        CancellationToken token);
    
    /// <summary>
    /// Проверяет существует ли комментарий с заданным идентификатором.
    /// </summary>
    /// <param name="id">Идентификатор комментария <see cref="Guid"/>.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns><code data-dev-comment-type="langword">true</code> если комментарий найден, иначе <code data-dev-comment-type="langword">false</code>.</returns>
    Task<bool> IsExistsAsync(Guid id, CancellationToken token);
    
    /// <summary>
    /// Проверяет существует ли комментарий с заданным идентификатором у объявления.
    /// </summary>
    /// <param name="advertId">Идентификатор Объявления <see cref="Guid"/>.</param>
    /// <param name="commentId">Идентификатор комментария <see cref="Guid"/>.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns><code data-dev-comment-type="langword">true</code> если комментарий найден, иначе <code data-dev-comment-type="langword">false</code>.</returns>
    Task<bool> IsExistsAsync(Guid advertId, Guid commentId, CancellationToken token);
    
    /// <summary>
    /// Метод для получения идентификатора пользователя комментария.
    /// </summary>
    /// <param name="id">Идентификатор комментария.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns>Идентификатор пользователя.</returns>
    Task<Guid> GetUserIdAsync(Guid id, CancellationToken token);
    
    /// <summary>
    /// Метод для получения даты и времени удаления комментария.
    /// </summary>
    /// <param name="id">Идентификатор комментария.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns>Дата и время удаления, если комментарий был удален, иначе null.</returns>
    Task<DateTime?> GetDeletedAtAsync(Guid id, CancellationToken token);
}