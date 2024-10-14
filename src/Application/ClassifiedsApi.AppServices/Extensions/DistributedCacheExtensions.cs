using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace ClassifiedsApi.AppServices.Extensions;

/// <summary>
/// Расширения для распределенного кэша <see cref="IDistributedCache"/>.
/// </summary>
public static class DistributedCacheExtensions
{
    private static readonly TimeSpan DefaultExpirationTime = TimeSpan.FromMinutes(5);
    
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

    /// <summary>
    /// Метод для добавления значения в кэш.
    /// </summary>
    /// <param name="cache">Распределенный кэш.</param>
    /// <param name="key">Ключ.</param>
    /// <param name="value">Значение.</param>
    /// <param name="expirationTime">Время жизни.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <typeparam name="T">Тип добавляемого значения.</typeparam>
    /// <returns></returns>
    public static Task SetAsync<T>(
        this IDistributedCache cache,
        string key, 
        T value, 
        TimeSpan? expirationTime = null, 
        CancellationToken token = default) where T : class
    {
        var jsonString = Serialize(value);
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expirationTime ?? DefaultExpirationTime
        };
        return cache.SetStringAsync(key, jsonString, options, token);
    }

    /// <summary>
    /// Метод для добавления значения в кэш.
    /// </summary>
    /// <param name="cache">Распределенный кэш.</param>
    /// <param name="label">Метка.</param>
    /// <param name="uniqueKey">Уникальный ключ.</param>
    /// <param name="value">Значение.</param>
    /// <param name="expirationTime">Время жизни.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <typeparam name="T">Тип добавляемого значения.</typeparam>
    /// <returns></returns>
    public static Task SetAsync<T>(
        this IDistributedCache cache,
        string label, 
        Guid uniqueKey, 
        T value, TimeSpan? expirationTime = null, 
        CancellationToken token = default) where T : class
    {
        var key = GetKey(label, uniqueKey);
        return cache.SetAsync(key, value, expirationTime, token);
    }

    /// <summary>
    /// Метод для получения значения из кэша.
    /// </summary>
    /// <param name="cache">Распределенный кэш.</param>
    /// <param name="key">Ключ.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <typeparam name="T">Тип получаемого значения.</typeparam>
    /// <returns>Значение если найдено, иначе null.</returns>
    public static async Task<T?> GetAsync<T>(
        this IDistributedCache cache, 
        string key, 
        CancellationToken token) where T : class
    {
        var jsonString = await cache.GetStringAsync(key, token);
        if (string.IsNullOrEmpty(jsonString))
        {
            return null;
        }
        var value = Deserialize<T>(jsonString);
        return value;
    }

    /// <summary>
    /// Метод для получения значения из кэша.
    /// </summary>
    /// <param name="cache">Распределенный кэш.</param>
    /// <param name="label">Метка.</param>
    /// <param name="uniqueKey">Уникальный ключ.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <typeparam name="T">Тип получаемого значения.</typeparam>
    /// <returns>Значение если найдено, иначе null.</returns>
    public static Task<T?> GetAsync<T>(
        this IDistributedCache cache,
        string label, 
        Guid uniqueKey, 
        CancellationToken token) where T : class
    {
        var key = GetKey(label, uniqueKey);
        return cache.GetAsync<T>(key, token);
    }

    /// <summary>
    /// Метод для удаления значения из кэша.
    /// </summary>
    /// <param name="cache">Распределенный кэш.</param>
    /// <param name="label">Метка.</param>
    /// <param name="uniqueKey">Уникальный ключ.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    public static Task RemoveAsync(this IDistributedCache cache, string label, Guid uniqueKey, CancellationToken token)
    {
        var key = GetKey(label, uniqueKey);
        return cache.RemoveAsync(key, token);
    }
    
    /// <summary>
    /// Проверяет есть ли в кэше значение с указаным ключом.
    /// </summary>
    /// <param name="cache">Распределенный кэш.</param>
    /// <param name="key">Ключ.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns><code data-dev-comment-type="langword">true</code> если ключ найден, иначе <code data-dev-comment-type="langword">false</code>.</returns>
    public static async Task<bool> HasKeyAsync(this IDistributedCache cache, string key, CancellationToken token)
    {
        var jsonString = await cache.GetStringAsync(key, token);
        return !string.IsNullOrEmpty(jsonString);
    }
    
    /// <summary>
    /// Проверяет есть ли в кэше значение с указаным ключом.
    /// </summary>
    /// <param name="cache">Распределенный кэш.</param>
    /// <param name="label">Метка.</param>
    /// <param name="uniqueKey">Уникальный ключ.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns><code data-dev-comment-type="langword">true</code> если ключ найден, иначе <code data-dev-comment-type="langword">false</code>.</returns>
    public static Task<bool> HasKeyAsync(this IDistributedCache cache, string label, Guid uniqueKey, CancellationToken token)
    {
        var key = GetKey(label, uniqueKey);
        return cache.HasKeyAsync(key, token);
    }
}