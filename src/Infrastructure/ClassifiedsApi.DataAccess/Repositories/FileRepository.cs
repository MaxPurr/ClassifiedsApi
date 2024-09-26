using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ClassifiedsApi.AppServices.Contexts.Files.Repositories;
using ClassifiedsApi.AppServices.Exceptions.Files;
using ClassifiedsApi.Contracts.Contexts.Files;
using ClassifiedsApi.DataAccess.Helpers;
using ClassifiedsApi.Infrastructure.Repository.GridFs;

namespace ClassifiedsApi.DataAccess.Repositories;

public class FileRepository : IFileRepository
{
    private readonly IGridFsRepository _repository;
    private readonly IMapper _mapper;

    public FileRepository(IGridFsRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    public async Task<string> UploadAsync(FileUpload fileUpload, CancellationToken token)
    {
        var objectId = await _repository.UploadAsync(fileUpload.Name, fileUpload.ReadStream, fileUpload.ContentType, token);
        return MongoDbHelper.ParseStringFromObjectId(objectId);
    }

    public async Task<FileInfo> GetFileInfoAsync(string id, CancellationToken token)
    {
        var objectId = MongoDbHelper.ParseObjectIdFromString(id);
        var fileInfo = await _repository.GetInfoByIdAsync(objectId, token);
        if (fileInfo == null)
        {
            throw new FileNotFoundException();
        }
        return _mapper.Map<FileInfo>(fileInfo);
    }

    public async Task<FileDownload> DownloadAsync(string id, CancellationToken token)
    {
        var objectId = MongoDbHelper.ParseObjectIdFromString(id);
        var downloadStream = await _repository.GetDownloadStreamAsync(objectId, token);
        if (downloadStream == null)
        {
            throw new FileNotFoundException();
        }
        return _mapper.Map<FileDownload>(downloadStream);
    }

    public async Task DeleteAsync(string id, CancellationToken token)
    {
        var objectId = MongoDbHelper.ParseObjectIdFromString(id);
        bool found = await _repository.DeleteAsync(objectId, token);
        if (!found)
        {
            throw new FileNotFoundException();
        }
    }
}