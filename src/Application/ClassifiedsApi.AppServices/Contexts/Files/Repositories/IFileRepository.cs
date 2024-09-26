using System;
using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.Contracts.Contexts.Files;

namespace ClassifiedsApi.AppServices.Contexts.Files.Repositories;

/// <summary>
/// Репозиторий файлов.
/// </summary>
public interface IFileRepository
{
    /// <summary>
    /// Метод для загрузки файла в репозиторий.
    /// </summary>
    /// <param name="fileUpload">Модель загрузки файла <see cref="FileUpload"/>.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns>Идентификатор загруженного файла в виде строки <see cref="String"/>.</returns>
    Task<string> UploadAsync(FileUpload fileUpload, CancellationToken token);
    
    /// <summary>
    /// Метод для получения информации о файле по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор файла в виде строки <see cref="String"/>.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns>Модель информации о файле <see cref="FileInfo"/>.</returns>
    Task<FileInfo> GetFileInfoAsync(string id, CancellationToken token);
    
    /// <summary>
    /// Метод для скачивания файла из репозитория.
    /// </summary>
    /// <param name="id">Идентификатор файла в виде строки <see cref="String"/>.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns>Модель скачивания файла <see cref="FileDownload"/>.</returns>
    Task<FileDownload> DownloadAsync(string id, CancellationToken token);
    
    /// <summary>
    /// Метод для удаления файла из репозитория.
    /// </summary>
    /// <param name="id">Идентификатор файла в виде строки <see cref="String"/>.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    Task DeleteAsync(string id, CancellationToken token);
}