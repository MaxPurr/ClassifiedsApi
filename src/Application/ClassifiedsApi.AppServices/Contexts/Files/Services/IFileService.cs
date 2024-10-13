using System;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.Contracts.Contexts.Files;

namespace ClassifiedsApi.AppServices.Contexts.Files.Services;

/// <summary>
/// Сервис файлов.
/// </summary>
public interface IFileService
{
    /// <summary>
    /// Метод для загрузки файла.
    /// </summary>
    /// <param name="fileUpload">Модель загрузки файла на сервер <see cref="FileUpload"/>.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns>Идентификатор загруженного файла.</returns>
    Task<Guid> UploadAsync(FileUpload fileUpload, CancellationToken token);
    
    /// <summary>
    /// Метод для получения информации о файле по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор файла.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns>Модель информации о файле <see cref="FileInfo"/>.</returns>
    Task<FileInfo> GetInfoAsync(Guid id, CancellationToken token);
    
    /// <summary>
    /// Метод для скачивания файла.
    /// </summary>
    /// <param name="id">Идентификатор файла.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns>Модель скачивания файла с сервера <see cref="FileDownload"/>.</returns>
    Task<FileDownload> DownloadAsync(Guid id, CancellationToken token);
    
    /// <summary>
    /// Метод для удаления файла.
    /// </summary>
    /// <param name="id">Идентификатор файла.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    Task DeleteAsync(Guid id, CancellationToken token);
}