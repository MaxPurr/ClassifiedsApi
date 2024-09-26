using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ClassifiedsApi.AppServices.Contexts.Files.Repositories;
using ClassifiedsApi.AppServices.Exceptions.Files;
using ClassifiedsApi.Contracts.Contexts.Files;
using ClassifiedsApi.DataAccess.Helpers;
using ClassifiedsApi.Infrastructure.Repository.GridFs;

namespace ClassifiedsApi.DataAccess.Repositories;

/// <inheritdoc/>
public class FileRepository : IFileRepository
{
    private readonly IGridFsRepository _repository;
    private readonly IMapper _mapper;
    
    /// <summary>
    /// Инициализирует экземпляр класса <see cref="FileRepository"/>.
    /// </summary>
    /// <param name="repository">Глупый репозиторий для работы с GridFS <see cref="IGridFsRepository"/>.</param>
    /// <param name="mapper">Маппер объектов <see cref="IMapper"/>.</param>
    public FileRepository(IGridFsRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    /// <inheritdoc/>
    public async Task<string> UploadAsync(FileUpload fileUpload, CancellationToken token)
    {
        var objectId = await _repository.UploadAsync(fileUpload.Name, fileUpload.ReadStream, fileUpload.ContentType, token);
        return MongoDbHelper.ParseStringFromObjectId(objectId);
    }
    
    /// <inheritdoc/>
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
    
    /// <inheritdoc/>
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
    
    /// <inheritdoc/>
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