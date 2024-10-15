using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.AppServices.Common.Services;
using ClassifiedsApi.AppServices.Contexts.Adverts.Validators;
using ClassifiedsApi.AppServices.Contexts.Categories.Services;
using ClassifiedsApi.AppServices.Contexts.Comments.Builders;
using ClassifiedsApi.AppServices.Contexts.Comments.Repositories;
using ClassifiedsApi.AppServices.Contexts.Comments.Validators;
using ClassifiedsApi.AppServices.Contexts.Users.Validators;
using ClassifiedsApi.AppServices.Exceptions.Comments;
using ClassifiedsApi.Contracts.Contexts.Comments;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace ClassifiedsApi.AppServices.Contexts.Comments.Services;

/// <inheritdoc />
public class CommentService : ICommentService
{
    private readonly ICommentRepository _repository;
    private readonly ICommentSpecificationBuilder _specificationBuilder;
    private readonly IUserAccessValidator _userAccessValidator;
    private readonly IAdvertValidator _advertValidator;
    private readonly ICommentValidator _commentValidator;
    private readonly TimeProvider _timeProvider;
    
    private readonly ILogger<CategoryService> _logger;
    private readonly IStructuralLoggingService _logService;
    
    private readonly IValidator<CommentCreate> _createValidator;
    private readonly IValidator<CommentsSearch> _searchValidator;
    private readonly IValidator<CommentUpdate> _updateValidator;

    /// <summary>
    /// Инициализирует экземпляр класса <see cref="CommentService"/>.
    /// </summary>
    /// <param name="repository">Репозиторий комментариев <see cref="ICommentRepository"/>.</param>
    /// <param name="specificationBuilder">Строитель спецификаций для комментариев <see cref="ICommentSpecificationBuilder"/>.</param>
    /// <param name="userAccessValidator">Верификатор прав пользователя <see cref="IUserAccessValidator"/>.</param>
    /// <param name="advertValidator">Валидатор объявлений <see cref="IAdvertValidator"/>.</param>
    /// <param name="commentValidator">Валидатор комментариев <see cref="ICommentValidator"/>.</param>
    /// <param name="timeProvider">Провайдер времени <see cref="TimeProvider"/>.</param>
    /// <param name="logger">Логгер.</param>
    /// <param name="logService">Сервис структурного логирования <see cref="IStructuralLoggingService"/>.</param>
    /// <param name="searchValidator">Валидатор модели поиска комментариев.</param>
    /// <param name="createValidator">Валидатор модели создания комментария.</param>
    /// <param name="updateValidator">Валидатор модели обновления комментария.</param>
    public CommentService(
        ICommentRepository repository, 
        ICommentSpecificationBuilder specificationBuilder, 
        IUserAccessValidator userAccessValidator,
        IAdvertValidator advertValidator,
        ICommentValidator commentValidator,
        TimeProvider timeProvider,
        ILogger<CategoryService> logger, 
        IStructuralLoggingService logService,
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
        _logger = logger;
        _logService = logService;
        _commentValidator = commentValidator;
        _advertValidator = advertValidator;
        _userAccessValidator = userAccessValidator;
    }

    private async Task ValidateParentExistsAndThrowAsync(Guid advertId, Guid commentParentId, CancellationToken token)
    {
        var exists = await _commentValidator.ValidateExistsAsync(advertId, commentParentId, token);
        if (!exists)
        {
            throw new CommentNotFoundException("Объявление не содержит родительского комментария с указаным идентификатором.");
        }
    }
    
    /// <inheritdoc />
    public async Task<Guid> CreateAsync(Guid userId, Guid advertId, CommentCreate commentCreate, CancellationToken token)
    {
        using (_logService.PushProperty("UserId", userId))
        using (_logService.PushProperty("AdvertId", advertId))
        using (_logService.PushProperty("CommentCreate", commentCreate, true))
        {
            _logger.LogInformation("Запрос на создание комментария.");
            
            _createValidator.ValidateAndThrow(commentCreate);
            await _advertValidator.ValidateExistsAndThrowAsync(advertId, token);
            if (commentCreate.ParentId.HasValue)
            {
                await ValidateParentExistsAndThrowAsync(advertId, commentCreate.ParentId.Value, token);
            }
            
            var createRequest = new CommentCreateRequest
            {
                UserId = userId,
                AdvertId = advertId,
                CommentCreate = commentCreate
            };
            var id = await _repository.CreateAsync(createRequest, token);
            _logger.LogInformation("Комментарий успешно создан. Идентификатор комментария: {CommentId}", id);

            return id;
        }
    }
    
    /// <inheritdoc />
    public async Task<CommentInfo> GetByIdAsync(Guid id, CancellationToken token)
    {
        using var _ = _logService.PushProperty("CommentId", id);
        _logger.LogInformation("Получение информации о комментарии по идентификатору.");
        
        var info = await _repository.GetByIdAsync(id, token);
        _logger.LogInformation("Информация о комментарии успешно получена: {CommentInfo}.", info);

        return info;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyCollection<CommentInfo>> SearchAsync(Guid advertId, CommentsSearch search, CancellationToken token)
    {
        using (_logService.PushProperty("AdvertId", advertId))
        using (_logService.PushProperty("CommentsSearch", search))
        {
            _logger.LogInformation("Поиск комментариев по запросу.");
            
            _searchValidator.ValidateAndThrow(search);
            await _advertValidator.ValidateExistsAndThrowAsync(advertId, token);
            if (search.FilterByParentId != null &&
                search.FilterByParentId.ParentId.HasValue)
            {
                await ValidateParentExistsAndThrowAsync(advertId, search.FilterByParentId.ParentId.Value, token);
            }

            var specification = _specificationBuilder.Build(advertId, search);
            _logger.LogInformation("Построена спецификация поиска комментариев.");
            
            var comments = await _repository.GetBySpecificationWithPaginationAsync(
                specification: specification,
                skip: search.Skip,
                take: search.Take.GetValueOrDefault(),
                order: search.Order!,
                token: token);
            _logger.LogInformation("Комментарии успешно получены.");
            
            return comments;
        }
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid userId, Guid commentId, CancellationToken token)
    {
        using (_logService.PushProperty("UserId", userId))
        using (_logService.PushProperty("CommentId", commentId))
        {
            _logger.LogInformation("Запрос на удаление комментария.");
            
            await _userAccessValidator.ValidateCommentAccessAndThrowAsync(userId, commentId, token);
            await _commentValidator.ValidateIsNotDeletedAndThrowAsync(commentId, token);
       
            var deletedAt = _timeProvider.GetUtcNow().UtcDateTime;
            await _repository.DeleteAsync(commentId, deletedAt, token);
            _logger.LogInformation("Комментарий успешно удален.");
        }
    }

    /// <inheritdoc />
    public async Task<CommentInfo> UpdateAsync(Guid userId, Guid commentId, CommentUpdate commentUpdate, CancellationToken token)
    {
        using (_logService.PushProperty("UserId", userId))
        using (_logService.PushProperty("CommentId", commentId))
        using (_logService.PushProperty("CommentUpdate", commentUpdate))
        {
            _logger.LogInformation("Запрос на удаление комментария.");
            
            _updateValidator.ValidateAndThrow(commentUpdate);
            await _userAccessValidator.ValidateCommentAccessAndThrowAsync(userId, commentId, token);
            await _commentValidator.ValidateIsNotDeletedAndThrowAsync(commentId, token);
       
            var updatedAt = _timeProvider.GetUtcNow().UtcDateTime;
            var updatedComment = await _repository.UpdateAsync(commentId, commentUpdate, updatedAt, token);
            _logger.LogInformation("Комментарий успешно обновлен.");
            
            return updatedComment;
        }
    }
}