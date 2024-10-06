using ClassifiedsApi.AppServices.Exceptions.Common;
using MongoDB.Bson;

namespace ClassifiedsApi.DataAccess.Helpers;

public static class MongoDbHelper
{
    public static ObjectId ParseObjectIdFromString(string id)
    {
        if (ObjectId.TryParse(id, out var objectId))
        {
            return objectId;
        }
        throw new InvalidObjectIdFormatException();
    }

    public static string ParseStringFromObjectId(ObjectId id)
    {
        return id.ToString();
    }
}