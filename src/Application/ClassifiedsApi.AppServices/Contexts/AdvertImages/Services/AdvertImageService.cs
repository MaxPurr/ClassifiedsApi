using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.AppServices.Common.Services;
using ClassifiedsApi.AppServices.Contexts.AdvertImages.Repositories;
using ClassifiedsApi.AppServices.Contexts.Files.Services;
using ClassifiedsApi.AppServices.Contexts.Users.Services;
using ClassifiedsApi.AppServices.Extensions;
using ClassifiedsApi.Contracts.Contexts.Files;
using Microsoft.Extensions.Caching.Distributed;

namespace ClassifiedsApi.AppServices.Contexts.AdvertImages.Services;

/// <inheritdoc cref="IAdvertImageService"/>
public class AdvertImageService : ServiceBase, IAdvertImageService
{
    private const string CacheKeyFormat = "advert:{0}:images";
    private static readonly TimeSpan CacheExpirationTime = TimeSpan.FromMinutes(5);
    
    private readonly IAdvertImageRepository _advertImageRepository;
    private readonly IFileService _fileService;
    private readonly IDistributedCache _cache;
    private readonly IUserAccessVerifier _userAccessVerifier;
    private readonly IFileVerifier _fileVerifier;

    /// <summary>
    /// Инициализирует экземпляр класса <see cref="AdvertImageService"/>.
    /// </summary>
    /// <param name="advertImageRepository">Репозиторий фотографий объявлений <see cref="IAdvertImageRepository"/>.</param>
    /// <param name="fileService">Сервис файлов <see cref="IFileService"/>.</param>
    /// <param name="cache">Распределенный кэш <see cref="IDistributedCache"/>.</param>
    /// <param name="userAccessVerifier">Верификатор прав пользователя <see cref="IUserAccessVerifier"/>.</param>
    /// <param name="fileVerifier">Верификатор файлов <see cref="IFileVerifier"/>.</param>
    public AdvertImageService(
        IAdvertImageRepository advertImageRepository,
        IFileService fileService,
        IDistributedCache cache,
        IUserAccessVerifier userAccessVerifier,
        IFileVerifier fileVerifier)
    {
        _advertImageRepository = advertImageRepository;
        _userAccessVerifier = userAccessVerifier;
        _fileService = fileService;
        _fileVerifier = fileVerifier;
        _cache = cache;
    }

    private static string GetCacheKey(Guid advertId)
    {
        return string.Format(CacheKeyFormat, advertId);
    }

    private Task ClearCacheAsync(Guid advertId, CancellationToken token)
    {
        var cacheKey = GetCacheKey(advertId);
        return _cache.RemoveAsync(cacheKey, token);
    }
    
    /// <inheritdoc />
    public async Task<Guid> UploadAsync(Guid userId, Guid advertId, FileUpload imageUpload, CancellationToken token)
    {
        await _userAccessVerifier.VerifyAdvertAccessAndThrowAsync(userId, advertId, token);
        _fileVerifier.VerifyImageContentTypeAndThrow(imageUpload.ContentType);
        
        await ClearCacheAsync(advertId, token);
        
        using var scope = CreateTransactionScope();
        var imageId = await _fileService.UploadAsync(imageUpload, token);
        await _advertImageRepository.AddAsync(advertId, imageId, token);
        scope.Complete();
        return imageId;
    }
    
    /// <inheritdoc />
    public async Task DeleteAsync(Guid userId, Guid advertId, Guid imageId, CancellationToken token)
    {
        await _userAccessVerifier.VerifyAdvertAccessAndThrowAsync(userId, advertId, token);
        
        await ClearCacheAsync(advertId, token);
        
        using var scope = CreateTransactionScope();
        await _advertImageRepository.DeleteAsync(advertId, imageId, token);
        await _fileService.DeleteAsync(imageId, token);
        scope.Complete();
    }
    
    /// <inheritdoc />
    public async Task<IReadOnlyCollection<Guid>> GetByAdvertIdAsync(Guid advertId, CancellationToken token)
    {
        var cacheKey = GetCacheKey(advertId);
        var ids = await _cache.GetAsync<IReadOnlyCollection<Guid>>(cacheKey, token);
        if (ids != null)
        {
            return ids;
        }
        ids = await _advertImageRepository.GetByAdvertIdAsync(advertId, token);
        await _cache.SetAsync(cacheKey, ids, CacheExpirationTime, token);
        return ids;
    }

    /// <inheritdoc />
    public async Task DeleteByAdvertIdAsync(Guid advertId, CancellationToken token)
    {
        await ClearCacheAsync(advertId, token);
        
        using var scope = CreateTransactionScope();
        var imageIds = await _advertImageRepository.DeleteByAdvertIdAsync(advertId, token);
        await _fileService.DeleteRangeAsync(imageIds.ToList(), token);
        scope.Complete();
    }
}