using MongoDB.Bson;
using MongoDB.Driver;

namespace mongo_scratch.Infrastructure;

public interface IReportMongoContext : IMongoContext
{
    IMongoCollection<T> GetCollection<T>();

    IMongoCollection<BsonDocument> GetGenericCollection<T>();
}