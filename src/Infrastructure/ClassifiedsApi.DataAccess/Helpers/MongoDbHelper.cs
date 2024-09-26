using System;
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
        throw new ArgumentException("Неверный формат id.");
    }

    public static string ParseStringFromObjectId(ObjectId id)
    {
        return id.ToString();
    }
}