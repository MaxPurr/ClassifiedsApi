using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.AppServices.Contexts.Files.Repositories;
using ClassifiedsApi.Contracts.Contexts.Files;

namespace ClassifiedsApi.AppServices.Contexts.Files.Services;

/// <inheritdoc/>
public class FileService : IFileService
{
    private readonly IFileRepository _repository;
    
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="FileService"/>.
    /// </summary>
    /// <param name="repository">Репозиторий файлов <see cref="IFileRepository"/>.</param>
    public FileService(IFileRepository repository)
    {
        _repository = repository;
    }
    
    /// <inheritdoc/>
    public Task<string> UploadAsync(FileUpload fileUpload, CancellationToken token)
    {
        return _repository.UploadAsync(fileUpload, token);
    }
    
    /// <inheritdoc/>
    public Task<FileInfo> GetFileInfoAsync(string id, CancellationToken token)
    {
        return _repository.GetFileInfoAsync(id, token);
    }
    
    /// <inheritdoc/>
    public Task<FileDownload> DownloadAsync(string id, CancellationToken token)
    {
        return _repository.DownloadAsync(id, token);
    }
    
    /// <inheritdoc/>
    public Task DeleteAsync(string id, CancellationToken token)
    {
        return _repository.DeleteAsync(id, token);
    }
}