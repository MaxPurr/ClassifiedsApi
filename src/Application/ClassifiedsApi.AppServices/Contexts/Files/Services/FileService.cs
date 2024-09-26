using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.AppServices.Contexts.Files.Repositories;
using ClassifiedsApi.Contracts.Contexts.Files;

namespace ClassifiedsApi.AppServices.Contexts.Files.Services;

public class FileService : IFileService
{
    private readonly IFileRepository _repository;

    public FileService(IFileRepository repository)
    {
        _repository = repository;
    }

    public Task<string> UploadAsync(FileUpload fileUpload, CancellationToken token)
    {
        return _repository.UploadAsync(fileUpload, token);
    }

    public Task<FileInfo> GetFileInfoAsync(string id, CancellationToken token)
    {
        return _repository.GetFileInfoAsync(id, token);
    }

    public Task<FileDownload> DownloadAsync(string id, CancellationToken token)
    {
        return _repository.DownloadAsync(id, token);
    }

    public Task DeleteAsync(string id, CancellationToken token)
    {
        return _repository.DeleteAsync(id, token);
    }
}