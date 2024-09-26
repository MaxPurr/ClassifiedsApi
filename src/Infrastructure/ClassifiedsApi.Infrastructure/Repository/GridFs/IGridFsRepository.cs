using MongoDB.Bson;
using MongoDB.Driver.GridFS;

namespace ClassifiedsApi.Infrastructure.Repository.GridFs;

public interface IGridFsRepository
{
    Task<ObjectId> UploadAsync(string fileName, Stream source, string contentType, CancellationToken token);
    
    Task<bool> DeleteAsync(ObjectId id, CancellationToken token);
    
    Task<GridFSFileInfo?> GetInfoByIdAsync(ObjectId id, CancellationToken token);
    
    Task<GridFSDownloadStream<ObjectId>?> GetDownloadStreamAsync(ObjectId id, CancellationToken token);
}