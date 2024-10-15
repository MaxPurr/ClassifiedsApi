using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.AppServices.Common.Services;
using Microsoft.Extensions.Caching.Distributed;

namespace ClassifiedsApi.Infrastructure.Services.Cache;

/// <inheritdoc />
public class SerializableCache : ISerializableCache
{
    private static readonly TimeSpan DefaultExpirationTime = TimeSpan.FromMinutes(5);
    
    private readonly IDistributedCache _cache;

    /// <summary>
    /// Инициализирует экземпляр класса <see cref="SerializableCache"/>.
    /// </summary>
    /// <param name="cache">Распределенный кэш <see cref="IDistributedCache"/>.</param>
    public SerializableCache(IDistributedCache cache)
    {
        _cache = cache;
    }
    
    private static string Serialize<T>(T value)
    {
        return JsonSerializer.Serialize(value);
    }
    
    private static T Deserialize<T>(string jsonString)
    {
        return JsonSerializer.Deserialize<T>(jsonString)!;
    }
    
    private static string GetKey(string label, Guid uniqueKey)
    {
        return $"{label}:{uniqueKey.ToString()}";
    }
    
    /// <inheritdoc />
    public Task SetAsync<T>(string key, T value, TimeSpan? expirationTime = null, CancellationToken token = default) where T : class
    {
        var jsonString = Serialize(value);
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expirationTime ?? DefaultExpirationTime
        };
        return _cache.SetStringAsync(key, jsonString, options, token);
    }
    
    /// <inheritdoc />
    public Task SetAsync<T>(string label, Guid uniqueKey, T value, TimeSpan? expirationTime = null,
        CancellationToken token = default) where T : class
    {
        var key = GetKey(label, uniqueKey);
        return SetAsync(key, value, expirationTime, token);
    }

    /// <inheritdoc />
    public async Task<T?> GetAsync<T>(string key, CancellationToken token) where T : class
    {
        var jsonString = await _cache.GetStringAsync(key, token);
        if (string.IsNullOrEmpty(jsonString))
        {
            return null;
        }
        var value = Deserialize<T>(jsonString);
        return value;
    }

    /// <inheritdoc />
    public Task<T?> GetAsync<T>(string label, Guid uniqueKey, CancellationToken token) where T : class
    {
        var key = GetKey(label, uniqueKey);
        return GetAsync<T>(key, token);
    }

    /// <inheritdoc />
    public Task RemoveAsync(string label, Guid uniqueKey, CancellationToken token)
    {
        var key = GetKey(label, uniqueKey);
        return RemoveAsync(key, token);
    }

    /// <inheritdoc />
    public Task RemoveAsync(string key, CancellationToken token)
    {
        return _cache.RemoveAsync(key, token);
    }

    /// <inheritdoc />
    public async Task<bool> HasKeyAsync(string key, CancellationToken token)
    {
        var jsonString = await _cache.GetStringAsync(key, token);
        return !string.IsNullOrEmpty(jsonString);
    }

    /// <inheritdoc />
    public Task<bool> HasKeyAsync(string label, Guid uniqueKey, CancellationToken token)
    {
        var key = GetKey(label, uniqueKey);
        return HasKeyAsync(key, token);
    }
}