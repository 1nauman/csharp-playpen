using MongoDB.Driver;

namespace mongo_scratch.Infrastructure;

public interface IMongoContext
{
    IMongoCollection<T> GetCollection<T>();
}