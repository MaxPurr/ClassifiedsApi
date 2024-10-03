using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver.GridFS;

namespace ClassifiedsApi.Infrastructure.Repository.GridFs;

/// <summary>
/// Глупый репозиторий для работы с GridFs.
/// </summary>
public interface IGridFsRepository
{
    /// <summary>
    /// Метод для загрузки файла.
    /// </summary>
    /// <param name="fileName">Имя файла <see cref="String"/>.</param>
    /// <param name="source">Поток для чтения файла <see cref="Stream"/>.</param>
    /// <param name="contentType">Тип контента <see cref="String"/>.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns>Идентификатор файла <see cref="ObjectId"/>.</returns>
    Task<ObjectId> UploadAsync(string fileName, Stream source, string contentType, CancellationToken token);
    
    /// <summary>
    /// Метод для удаления файла по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор файла <see cref="ObjectId"/>.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns><code data-dev-comment-type="langword">true</code> если файл найден, иначе <code data-dev-comment-type="langword">false</code>.</returns>
    Task<bool> DeleteAsync(ObjectId id, CancellationToken token);
    
    /// <summary>
    /// Метод для получения информации о файле.
    /// </summary>
    /// <param name="id">Идентификатор файла <see cref="ObjectId"/>.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns>Информацию о файле <see cref="GridFSFileInfo"/> если файл найден, иначе null.</returns>
    Task<GridFSFileInfo?> GetInfoByIdAsync(ObjectId id, CancellationToken token);
    
    /// <summary>
    /// Метод для получения потока для чтения файла.
    /// </summary>
    /// <param name="id">Идентификатор файла <see cref="ObjectId"/>.</param>
    /// <param name="token">Токен отмены операции <see cref="CancellationToken"/>.</param>
    /// <returns>Поток для чтения файла если файл найден, иначе null.</returns>
    Task<GridFSDownloadStream<ObjectId>?> GetDownloadStreamAsync(ObjectId id, CancellationToken token);
}