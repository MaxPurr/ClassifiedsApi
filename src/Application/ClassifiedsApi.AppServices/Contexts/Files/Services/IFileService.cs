using System.Threading;
using System.Threading.Tasks;
using ClassifiedsApi.Contracts.Contexts.Files;

namespace ClassifiedsApi.AppServices.Contexts.Files.Services;

public interface IFileService
{
    Task<string> UploadAsync(FileUpload fileUpload, CancellationToken token);
    
    Task<FileInfo> GetFileInfoAsync(string id, CancellationToken token);
    
    Task<FileDownload> DownloadAsync(string id, CancellationToken token);
    
    Task DeleteAsync(string id, CancellationToken token);
}