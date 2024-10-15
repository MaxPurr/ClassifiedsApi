using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.AppServices.Common.Services;
using ClassifiedsApi.AppServices.Contexts.AdvertImages.Repositories;
using ClassifiedsApi.AppServices.Contexts.Files.Services;
using ClassifiedsApi.AppServices.Contexts.Files.Validators;
using ClassifiedsApi.AppServices.Contexts.Users.Validators;
using ClassifiedsApi.AppServices.Extensions;
using ClassifiedsApi.Contracts.Contexts.Files;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace ClassifiedsApi.AppServices.Contexts.AdvertImages.Services;

/// <inheritdoc cref="IAdvertImageService"/>
public class AdvertImageService : ServiceBase, IAdvertImageService
{
    private const string CacheKeyFormat = "advert:{0}:images";
    private static readonly TimeSpan CacheExpirationTime = TimeSpan.FromMinutes(5);
    
    private readonly IAdvertImageRepository _advertImageRepository;
    private readonly IFileService _fileService;
    private readonly ISerializableCache _cache;
    private readonly IUserAccessValidator _userAccessValidator;
    private readonly IFileValidator _fileValidator;
    
    private readonly ILogger<AdvertImageService> _logger;
    private readonly IStructuralLoggingService _logService;

    /// <summary>
    /// Инициализирует экземпляр класса <see cref="AdvertImageService"/>.
    /// </summary>
    /// <param name="advertImageRepository">Репозиторий фотографий объявлений <see cref="IAdvertImageRepository"/>.</param>
    /// <param name="fileService">Сервис файлов <see cref="IFileService"/>.</param>
    /// <param name="cache">Сериализуемый кэш <see cref="ISerializableCache"/>.</param>
    /// <param name="userAccessValidator">Валидатор прав пользователей <see cref="IUserAccessValidator"/>.</param>
    /// <param name="fileValidator">Валидатор файлов <see cref="IFileValidator"/>.</param>
    /// <param name="logger">Логгер.</param>
    /// <param name="logService">Сервис структурного логирования <see cref="IStructuralLoggingService"/>.</param>
    public AdvertImageService(
        IAdvertImageRepository advertImageRepository,
        IFileService fileService,
        ISerializableCache cache,
        IUserAccessValidator userAccessValidator,
        IFileValidator fileValidator,
        ILogger<AdvertImageService> logger, 
        IStructuralLoggingService logService)
    {
        _advertImageRepository = advertImageRepository;
        _userAccessValidator = userAccessValidator;
        _fileService = fileService;
        _fileValidator = fileValidator;
        _logger = logger;
        _logService = logService;
        _cache = cache;
    }

    private static string GetCacheKey(Guid advertId)
    {
        return string.Format(CacheKeyFormat, advertId);
    }

    private async Task ClearCacheAsync(Guid advertId, CancellationToken token)
    {
        var cacheKey = GetCacheKey(advertId);
        await _cache.RemoveAsync(cacheKey, token);
        _logger.LogInformation("Кэш фотографий объявления очищен.");
    }
    
    /// <inheritdoc />
    public async Task<Guid> UploadAsync(Guid userId, Guid advertId, FileUpload imageUpload, CancellationToken token)
    {
        using (_logService.PushProperty("UserId", userId))
        using (_logService.PushProperty("AdvertId", advertId))
        {
            _logger.LogInformation("Запрос на добавление фотографии объявления.");
            
            _fileValidator.ValidateImageContentTypeAndThrow(imageUpload.ContentType);
            await _userAccessValidator.ValidateAdvertAccessAndThrowAsync(userId, advertId, token);
        
            await ClearCacheAsync(advertId, token);
        
            using var scope = CreateTransactionScope();
            var imageId = await _fileService.UploadAsync(imageUpload, token);
            await _advertImageRepository.AddAsync(advertId, imageId, token);
            scope.Complete();
            _logger.LogInformation("Фотография объявления успешно добавлена. Идентификатор фотографии: {ImageId}", imageId);
            
            return imageId;
        }
    }
    
    /// <inheritdoc />
    public async Task DeleteAsync(Guid userId, Guid advertId, Guid imageId, CancellationToken token)
    {
        using (_logService.PushProperty("UserId", userId))
        using (_logService.PushProperty("AdvertId", advertId))
        using (_logService.PushProperty("ImageId", imageId))
        {
            _logger.LogInformation("Запрос на удаление фотографии объявления.");
            
            await _userAccessValidator.ValidateAdvertAccessAndThrowAsync(userId, advertId, token);
        
            await ClearCacheAsync(advertId, token);
        
            using var scope = CreateTransactionScope();
            await _advertImageRepository.DeleteAsync(advertId, imageId, token);
            await _fileService.DeleteAsync(imageId, token);
            scope.Complete();
            
            _logger.LogInformation("Фотография объявления успешно удалена.");
        }
    }
    
    /// <inheritdoc />
    public async Task<IReadOnlyCollection<Guid>> GetByAdvertIdAsync(Guid advertId, CancellationToken token)
    {
        _logger.LogInformation("Получение идентификаторов фотографий объявления.");
        
        var cacheKey = GetCacheKey(advertId);
        var ids = await _cache.GetAsync<IReadOnlyCollection<Guid>>(cacheKey, token);
        if (ids != null)
        {
            _logger.LogInformation("Идентификаторы фотографий объявления получены из кэша.");
            return ids;
        }
        
        ids = await _advertImageRepository.GetByAdvertIdAsync(advertId, token);
        _logger.LogInformation("Идентификаторы фотографий объявления получены из базы данных.");
        
        await _cache.SetAsync(cacheKey, ids, CacheExpirationTime, token);
        _logger.LogInformation("Идентификаторы фотографий объявления добавлены в кэш.");
        
        return ids;
    }

    /// <inheritdoc />
    public async Task DeleteByAdvertIdAsync(Guid advertId, CancellationToken token)
    {
        await ClearCacheAsync(advertId, token);
        
        _logger.LogInformation("Запрос на удаление фотографий объявления.");
        
        using var scope = CreateTransactionScope();
        var imageIds = await _advertImageRepository.DeleteByAdvertIdAsync(advertId, token);
        await _fileService.DeleteRangeAsync(imageIds.ToList(), token);
        scope.Complete();
        
        _logger.LogInformation("Фотографии объявления успешно удалены.");
    }
}