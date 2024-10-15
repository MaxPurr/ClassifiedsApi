using System;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.AppServices.Common.Services;
using ClassifiedsApi.AppServices.Contexts.Files.Services;
using ClassifiedsApi.AppServices.Contexts.Files.Validators;
using ClassifiedsApi.AppServices.Contexts.Users.Repositories;
using ClassifiedsApi.Contracts.Contexts.Files;
using ClassifiedsApi.Contracts.Contexts.Users;
using Microsoft.Extensions.Logging;

namespace ClassifiedsApi.AppServices.Contexts.Users.Services;

/// <inheritdoc cref="IUserService"/>
public class UserService : ServiceBase, IUserService
{
    private readonly IUserRepository _repository;
    private readonly IFileService _fileService;
    private readonly IFileValidator _fileValidator;
    
    private readonly ILogger<UserService> _logger;
    private readonly IStructuralLoggingService _logService;

    /// <summary>
    /// Инициализирует экземпляр класса <see cref="UserService"/>.
    /// </summary>
    /// <param name="repository">Репозиторий пользователей <see cref="IUserRepository"/>.</param>
    /// <param name="fileService">Сервис файлов <see cref="IFileService"/>.</param>
    /// <param name="fileValidator">Валидатор файлов <see cref="IFileValidator"/>.</param>
    /// <param name="logger">Логгер.</param>
    /// <param name="logService">Сервис структурного логирования <see cref="IStructuralLoggingService"/>.</param>
    public UserService(
        IUserRepository repository, 
        IFileService fileService, 
        IFileValidator fileValidator,
        ILogger<UserService> logger, 
        IStructuralLoggingService logService)
    {
        _repository = repository;
        _fileService = fileService;
        _fileValidator = fileValidator;
        _logger = logger;
        _logService = logService;
    }
    
    /// <inheritdoc />
    public async Task<UserInfo> GetByIdAsync(Guid id, CancellationToken token)
    {
        using var _ = _logService.PushProperty("UserId", id);
        _logger.LogInformation("Получение информации о пользователе по идентификатору.");
        
        var info = await _repository.GetByIdAsync(id, token);
        _logger.LogInformation("Информация о пользователе успешно получена: {@UserInfo}", info);
        
        return info;
    }
    
    /// <inheritdoc />
    public async Task<UserContactsInfo> GetContactsInfoAsync(Guid id, CancellationToken token)
    {
        using var _ = _logService.PushProperty("UserId", id);
        _logger.LogInformation("Получение информации о контактах пользователя по идентификатору.");
        
        var info = await _repository.GetContactsInfoAsync(id, token);
        _logger.LogInformation("Информация о контактах пользователя успешно получена: {@UserContactsInfo}", info);

        return info;
    }

    /// <inheritdoc />
    public async Task<Guid> UpdatePhotoAsync(Guid id, FileUpload photoUpload, CancellationToken token)
    {
        using var _ = _logService.PushProperty("UserId", id);

        _logger.LogInformation("Запрос на обновление фотографии пользователя.");

        _fileValidator.ValidateImageContentTypeAndThrow(photoUpload.ContentType);

        using var scope = CreateTransactionScope();
        var photoId = await _fileService.UploadAsync(photoUpload, token);
        var prevPhotoId = await _repository.UpdatePhotoAsync(id, photoId, token);
        if (prevPhotoId.HasValue)
        {
            await _fileService.DeleteAsync(prevPhotoId.Value, token);
            _logger.LogInformation("Старая фотография пользователя удалена. " +
                                   "Идентификатор удаленной фотографии: {DeletedPhotoId}", prevPhotoId.Value);
        }
        scope.Complete();
        _logger.LogInformation("Фотография пользователя успешно обновлена. Идентификатор фотографии: {PhotoId}", photoId);
        
        return photoId;
    }

    /// <inheritdoc />
    public async Task DeletePhotoAsync(Guid id, CancellationToken token)
    {
        using var _ = _logService.PushProperty("UserId", id);
        _logger.LogInformation("Запрос на удаление фотографии пользователя.");
        
        using var scope = CreateTransactionScope();
        var photoId = await _repository.DeletePhotoAsync(id, token);
        await _fileService.DeleteAsync(photoId, token);
        scope.Complete();
        
        _logger.LogInformation("Фотография пользователя успешно удалена.");
    }
}