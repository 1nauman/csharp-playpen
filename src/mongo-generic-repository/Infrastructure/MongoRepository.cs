using mongo_generic_repository.Seedwork;
using MongoDB.Driver;

namespace mongo_generic_repository.Infrastructure;

public class MongoRepository<TEntity, TId> : IRepository<TEntity, TId> where TEntity : IEntity<TId>
{
    private readonly IMongoCollection<TEntity> _collection;

    public MongoRepository(string connectionString, string databaseName, string collectionName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);
        ArgumentException.ThrowIfNullOrWhiteSpace(databaseName);
        ArgumentException.ThrowIfNullOrWhiteSpace(collectionName);
        
        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);
        _collection = database.GetCollection<TEntity>(collectionName);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync() =>
        await _collection.Find(_ => true).ToListAsync();

    public async Task<TEntity> GetByIdAsync(TId id)
    {
        var filter = Builders<TEntity>.Filter.Eq(e => e.Id, id);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task CreateAsync(TEntity entity) =>
        await _collection.InsertOneAsync(entity);

    public async Task UpdateAsync(TEntity entity)
    {
        var filter = Builders<TEntity>.Filter.Eq(e => e.Id, entity.Id);
        await _collection.ReplaceOneAsync(filter, entity);
    }

    public async Task DeleteAsync(TId id)
    {
        var filter = Builders<TEntity>.Filter.Eq(e => e.Id, id);
        await _collection.DeleteOneAsync(filter);
    }
}