using System;
using System.Threading;
using System.Threading.Tasks;

namespace ClassifiedsApi.AppServices.Common.Services;

/// <summary>
/// Сериализуемый кэш.
/// </summary>
public interface ISerializableCache
{
    /// <summary>
    /// Метод для добавления значения в кэш.
    /// </summary>
    /// <param name="key">Ключ.</param>
    /// <param name="value">Значение.</param>
    /// <param name="expirationTime">Время жизни.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <typeparam name="T">Тип добавляемого значения.</typeparam>
    /// <returns></returns>
    Task SetAsync<T>(
        string key,
        T value,
        TimeSpan? expirationTime = null,
        CancellationToken token = default) where T : class;

    /// <summary>
    /// Метод для добавления значения в кэш.
    /// </summary>
    /// <param name="label">Метка.</param>
    /// <param name="uniqueKey">Уникальный ключ.</param>
    /// <param name="value">Значение.</param>
    /// <param name="expirationTime">Время жизни.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <typeparam name="T">Тип добавляемого значения.</typeparam>
    /// <returns></returns>
    Task SetAsync<T>(
        string label, 
        Guid uniqueKey, 
        T value, 
        TimeSpan? expirationTime = null, 
        CancellationToken token = default) where T : class;

    /// <summary>
    /// Метод для получения значения из кэша.
    /// </summary>
    /// <param name="key">Ключ.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <typeparam name="T">Тип получаемого значения.</typeparam>
    /// <returns>Значение если найдено, иначе null.</returns>
    Task<T?> GetAsync<T>(
        string key,
        CancellationToken token) where T : class;

    /// <summary>
    /// Метод для получения значения из кэша.
    /// </summary>
    /// <param name="label">Метка.</param>
    /// <param name="uniqueKey">Уникальный ключ.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <typeparam name="T">Тип получаемого значения.</typeparam>
    /// <returns>Значение если найдено, иначе null.</returns>
    Task<T?> GetAsync<T>(
        string label,
        Guid uniqueKey,
        CancellationToken token) where T : class;

    /// <summary>
    /// Метод для удаления значения из кэша.
    /// </summary>
    /// <param name="label">Метка.</param>
    /// <param name="uniqueKey">Уникальный ключ.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    Task RemoveAsync(string label, Guid uniqueKey, CancellationToken token);
    
    /// <summary>
    /// Метод для удаления значения из кэша.
    /// </summary>
    /// <param name="key">Ключ.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    Task RemoveAsync(string key, CancellationToken token);

    /// <summary>
    /// Проверяет есть ли в кэше значение с указаным ключом.
    /// </summary>
    /// <param name="key">Ключ.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns><code data-dev-comment-type="langword">true</code> если ключ найден, иначе <code data-dev-comment-type="langword">false</code>.</returns>
    Task<bool> HasKeyAsync(string key, CancellationToken token);

    /// <summary>
    /// Проверяет есть ли в кэше значение с указаным ключом.
    /// </summary>
    /// <param name="label">Метка.</param>
    /// <param name="uniqueKey">Уникальный ключ.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns><code data-dev-comment-type="langword">true</code> если ключ найден, иначе <code data-dev-comment-type="langword">false</code>.</returns>
    Task<bool> HasKeyAsync(string label, Guid uniqueKey, CancellationToken token);
}