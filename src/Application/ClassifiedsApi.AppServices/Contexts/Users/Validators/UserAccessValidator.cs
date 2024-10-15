using System;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.AppServices.Contexts.Adverts.Repositories;
using ClassifiedsApi.AppServices.Contexts.Comments.Repositories;
using ClassifiedsApi.AppServices.Exceptions.Users;

namespace ClassifiedsApi.AppServices.Contexts.Users.Validators;

/// <inheritdoc />
public class UserAccessValidator : IUserAccessValidator
{
    private readonly IAdvertRepository _advertRepository;
    private readonly ICommentRepository _commentRepository;

    /// <summary>
    /// Инициализирует экземпляр класса <see cref="UserAccessValidator"/>.
    /// </summary>
    /// <param name="advertRepository">Репозиторий объявлений <see cref="IAdvertRepository"/>.</param>
    /// <param name="commentRepository">Репозиторий комментариев <see cref="ICommentRepository"/>.</param>
    public UserAccessValidator(IAdvertRepository advertRepository, ICommentRepository commentRepository)
    {
        _advertRepository = advertRepository;
        _commentRepository = commentRepository;
    }

    /// <inheritdoc />
    public async Task ValidateAdvertAccessAndThrowAsync(Guid userId, Guid advertId, CancellationToken token)
    {
        var advertUserId = await _advertRepository.GetUserIdAsync(advertId, token);
        if (advertUserId != userId)
        {
            throw new AdvertAccessDeniedException();
        }
    }
    
    /// <inheritdoc />
    public async Task ValidateCommentAccessAndThrowAsync(Guid userId, Guid commentId, CancellationToken token)
    {
        var commentUserId = await _commentRepository.GetUserIdAsync(commentId, token);
        if (commentUserId != userId)
        {
            throw new CommentAccessDeniedException();
        }
    }
}