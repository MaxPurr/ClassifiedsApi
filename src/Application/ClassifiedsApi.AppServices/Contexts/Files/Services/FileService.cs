using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.AppServices.Common.Services;
using ClassifiedsApi.AppServices.Contexts.Files.Repositories;
using ClassifiedsApi.Contracts.Contexts.Files;
using Microsoft.Extensions.Caching.Distributed;
using ClassifiedsApi.AppServices.Extensions;
using Microsoft.Extensions.Logging;

namespace ClassifiedsApi.AppServices.Contexts.Files.Services;

/// <inheritdoc/>
public class FileService : IFileService
{
    private const string CacheKeyFormat = "file:{0}:info";
    private static readonly TimeSpan CacheExpirationTime = TimeSpan.FromMinutes(5);
    
    private readonly IFileRepository _repository;
    private readonly ISerializableCache _cache;
    
    private readonly ILogger<FileService> _logger;
    private readonly IStructuralLoggingService _logService;

    /// <summary>
    /// Инициализирует экземпляр класса <see cref="FileService"/>.
    /// </summary>
    /// <param name="repository">Репозиторий файлов <see cref="IFileRepository"/>.</param>
    /// <param name="cache">Сериализуемый кэш <see cref="ISerializableCache"/>.</param>
    /// <param name="logger">Логгер</param>
    /// <param name="logService">Сервис структурного логирования <see cref="IStructuralLoggingService"/>.</param>
    public FileService(
        IFileRepository repository, 
        ISerializableCache cache, 
        ILogger<FileService> logger, 
        IStructuralLoggingService logService)
    {
        _repository = repository;
        _cache = cache;
        _logger = logger;
        _logService = logService;
    }

    private static string GetCacheKey(Guid id)
    {
        return string.Format(CacheKeyFormat, id);
    }

    private async Task ClearCacheAsync(Guid id, CancellationToken token)
    {
        var cacheKey = GetCacheKey(id);
        await _cache.RemoveAsync(cacheKey, token);
        _logger.LogInformation("Кэш файла очищен.");
    }

    /// <inheritdoc/>
    public async Task<Guid> UploadAsync(FileUpload fileUpload, CancellationToken token)
    {
        using var _ = _logService.PushProperty("FileUpload", fileUpload, true);
        _logger.LogInformation("Запрос на загрузку файла.");

        var id = await _repository.UploadAsync(fileUpload, token);
        _logger.LogInformation("Файл был успешно загружен. Идентификатор файла: {FileId}", id);

        return id;
    }

    /// <inheritdoc/>
    public async Task<FileInfo> GetInfoAsync(Guid id, CancellationToken token)
    {
        using var _ = _logService.PushProperty("FileId", id);
        _logger.LogInformation("Получение информации о файле по идентификатору.");
        
        var cacheKey = GetCacheKey(id);
        var info = await _cache.GetAsync<FileInfo>(cacheKey, token);
        if (info != null)
        {
            _logger.LogInformation("Информация о файле получена из кэша: {@Info}.", info);
            return info;
        }
        
        info = await _repository.GetInfoAsync(id, token);
        _logger.LogInformation("Информация о файле получена из базы данных: {@Info}.", info);
        
        await _cache.SetAsync(cacheKey, info, CacheExpirationTime, token);
        _logger.LogInformation("Информация о файле добавлена в кэш.");
        
        return info;
    }
    
    /// <inheritdoc/>
    public async Task<FileDownload> DownloadAsync(Guid id, CancellationToken token)
    {
        using var _ = _logService.PushProperty("FileId", id);
        _logger.LogInformation("Запрос на получение модели скачивания файла.");
        
        var fileDownload = await _repository.DownloadAsync(id, token);
        _logger.LogInformation("Модель скачивания файла успено получена: {@FileDownload}", fileDownload);
        
        return fileDownload;
    }
    
    /// <inheritdoc/>
    public async Task DeleteAsync(Guid id, CancellationToken token)
    {
        using var _ = _logService.PushProperty("FileId", id);
        _logger.LogInformation("Запрос на удаление файла.");
        
        await ClearCacheAsync(id, token);
        
        await _repository.DeleteAsync(id, token);
        _logger.LogInformation("Файл был успешно удален из базы данных.");
    }
    
    /// <inheritdoc/>
    public async Task DeleteRangeAsync(ICollection<Guid> ids, CancellationToken token)
    {
        using var _ = _logService.PushProperty("FileIds", ids, true);
        _logger.LogInformation("Запрос на удаление файлов.");
        
        foreach (var id in ids)
        {
            await ClearCacheAsync(id, token);
        }
        await _repository.DeleteRangeAsync(ids, token);
        _logger.LogInformation("Файлы успешно удалены из базы данных.");
    }
}