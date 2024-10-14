using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.AppServices.Contexts.Files.Repositories;
using ClassifiedsApi.Contracts.Contexts.Files;
using Microsoft.Extensions.Caching.Distributed;
using ClassifiedsApi.AppServices.Extensions;

namespace ClassifiedsApi.AppServices.Contexts.Files.Services;

/// <inheritdoc/>
public class FileService : IFileService
{
    private const string CacheKeyFormat = "file:{0}:info";
    private static readonly TimeSpan CacheExpirationTime = TimeSpan.FromMinutes(5);
    
    private readonly IFileRepository _repository;
    private readonly IDistributedCache _cache;

    /// <summary>
    /// Инициализирует экземпляр класса <see cref="FileService"/>.
    /// </summary>
    /// <param name="repository">Репозиторий файлов <see cref="IFileRepository"/>.</param>
    /// <param name="cache">Распределенный кэш <see cref="IDistributedCache"/>.</param>
    public FileService(IFileRepository repository, IDistributedCache cache)
    {
        _repository = repository;
        _cache = cache;
    }

    private static string GetCacheKey(Guid id)
    {
        return string.Format(CacheKeyFormat, id);
    }

    private Task ClearCacheAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);
        return _cache.RemoveAsync(cacheKey);
    }
    
    /// <inheritdoc/>
    public Task<Guid> UploadAsync(FileUpload fileUpload, CancellationToken token)
    {
        return _repository.UploadAsync(fileUpload, token);
    }
    
    /// <inheritdoc/>
    public async Task<FileInfo> GetInfoAsync(Guid id, CancellationToken token)
    {
        var cacheKey = GetCacheKey(id);
        var info = await _cache.GetAsync<FileInfo>(cacheKey, token);
        if (info != null)
        {
            return info;
        }
        info = await _repository.GetInfoAsync(id, token);
        await _cache.SetAsync(cacheKey, info, CacheExpirationTime, token);
        return info;
    }
    
    /// <inheritdoc/>
    public Task<FileDownload> DownloadAsync(Guid id, CancellationToken token)
    {
        return _repository.DownloadAsync(id, token);
    }
    
    /// <inheritdoc/>
    public async Task DeleteAsync(Guid id, CancellationToken token)
    {
        await ClearCacheAsync(id);
        await _repository.DeleteAsync(id, token);
    }
    
    /// <inheritdoc/>
    public async Task DeleteRangeAsync(ICollection<Guid> ids, CancellationToken token)
    {
        foreach (var id in ids)
        {
            await ClearCacheAsync(id);
        }
        await _repository.DeleteRangeAsync(ids, token);
    }
}