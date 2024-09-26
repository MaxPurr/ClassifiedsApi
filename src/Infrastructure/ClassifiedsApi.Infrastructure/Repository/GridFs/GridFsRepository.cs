using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace ClassifiedsApi.Infrastructure.Repository.GridFs;

public class GridFsRepository : IGridFsRepository
{
    private readonly IGridFSBucket _gridFsBucket;
    
    public GridFsRepository(IGridFSBucket gridFsBucket)
    {
        _gridFsBucket = gridFsBucket;
    }
    
    public Task<ObjectId> UploadAsync(string fileName, Stream source, string contentType, CancellationToken token)
    {
        var options = new GridFSUploadOptions()
        {
            Metadata = new BsonDocument { { "content-type", contentType } }
        };
        return _gridFsBucket.UploadFromStreamAsync(fileName, source, options, token);
    }

    public async Task<bool> DeleteAsync(ObjectId id, CancellationToken token)
    {
        var fileInfo = await GetInfoByIdAsync(id, token);
        if (fileInfo == null)
        {
            return false;
        }
        await _gridFsBucket.DeleteAsync(id, token);
        return true;
    }

    public async Task<GridFSFileInfo?> GetInfoByIdAsync(ObjectId id, CancellationToken token)
    {
        var filter = Builders<GridFSFileInfo>.Filter.Eq("_id", id);
        var fileInfoCursor = await _gridFsBucket.FindAsync(filter, cancellationToken: token);
        var fileInfo = await fileInfoCursor.FirstOrDefaultAsync(token);
        return fileInfo;
    }

    public async Task<GridFSDownloadStream<ObjectId>?> GetDownloadStreamAsync(ObjectId id, CancellationToken token)
    {
        try
        {
            var downloadStream = await _gridFsBucket.OpenDownloadStreamAsync(id, cancellationToken: token);
            return downloadStream;
        }
        catch (GridFSFileNotFoundException)
        {
            return null;
        }
    }
}