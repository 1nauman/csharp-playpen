using System.Linq.Expressions;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace mongo_scratch.Infrastructure;

public class EntityMongoRepository<TEntity, TId> : IEntityRepository<TEntity, TId>
    where TEntity : class, IEntity<TId>
    where TId : IComparable
{
    private static readonly string IdempotencyField = "idempotencyKeys";
    private readonly IReportMongoContext _dbContext;

    private readonly ILogger _logger;

    public EntityMongoRepository([NotNull] IReportMongoContext mongoContext, ILoggerFactory loggerFactory)
    {
        _dbContext = mongoContext ?? throw new ArgumentNullException(nameof(mongoContext));

        _logger = loggerFactory.CreateLogger(typeof(EntityMongoRepository<,>));
    }

    protected IMongoCollection<TEntity> Collection => _dbContext.GetCollection<TEntity>();

    protected IMongoCollection<BsonDocument> GenericCollection => _dbContext.GetGenericCollection<TEntity>();


    public async Task<TEntity> Create(Func<Task<TEntity>> entityFactory, string idempotencyKey = null,
        string operationName = null)
    {
        if (entityFactory == null) throw new ArgumentNullException(nameof(entityFactory));

        // var idempotencyKey = _dbContext.IdempotencyKey != null
        //     ? string.Join('-',
        //         new[] {_dbContext.IdempotencyKey, operationName, "insert"}.Where(item => item != null))
        //     : null;

        if (idempotencyKey != null)
        {
            var filter = Builders<BsonDocument>.Filter.Eq($"{IdempotencyField}.value", idempotencyKey);
            var doc = await GenericCollection.Find(filter).Limit(1).SingleOrDefaultAsync();

            if (doc != null)
            {
                _logger.LogDebug("Document with idempotencyKey {idempotencyKey} found, returning existing document",
                    idempotencyKey);
                doc.Remove(IdempotencyField);
                var state = BsonSerializer.Deserialize<TEntity>(doc);
                return state;
            }

            _logger.LogTrace("No document found with idempotencyKey {idempotencyKey}", idempotencyKey);
        }

        _logger.LogTrace("Adding new document to repository");
        var newAggregate = await entityFactory();

        var newDoc = newAggregate.ToBsonDocument();
        newDoc[$"{IdempotencyField}"] = string.IsNullOrWhiteSpace(idempotencyKey)
            ? new BsonArray()
            : new BsonArray
            {
                CreateIdempotentKeyDocument(idempotencyKey, DateTime.UtcNow)
            };

        try
        {
            await GenericCollection.InsertOneAsync(newDoc, new InsertOneOptions { BypassDocumentValidation = false });
        }
        catch (MongoCommandException e)
        {
            if (MongoErrorCodes.DuplicateKeyErrorCodes.Contains(e.Code))
            {
                _logger.LogInformation(e, "Trying to create a document with the same Id" +
                                          " : {@id}",
                    newAggregate.Id);
                throw new Exception(
                    $"Duplicate entity exception: Type: '{typeof(TEntity)}' Id: '{newAggregate.Id.ToString()}'");
            }

            throw;
        }

        return newAggregate;
    }

    public async Task Update(TId id, Func<TEntity, Task> updateAction, long? version = null, string idempotencyKey =
            null
        , string operationName = null)
    {
        if (id == null) throw new ArgumentNullException(nameof(id));
        if (updateAction == null) throw new ArgumentNullException(nameof(updateAction));

        // var idempotencyKey = _dbContext.IdempotencyKey != null
        //     ? string.Join('-',
        //         new[] { _dbContext.IdempotencyKey, operationName, "update" }.Where(item => item != null))
        //     : null;

        using var logScope = _logger.BeginScope(new Dictionary<string, object>
        {
            { "Id", id },
            { "idempotencyKey", idempotencyKey },
            { "version", version },
            { "Method", "Update" }
        });

        var filters = new List<FilterDefinition<BsonDocument>>
        {
            GetEntityIdFilter(id)
        };

        //_logger.LogDebug("filters applied : {0}", filters);
        var doc = await GenericCollection.Find(Builders<BsonDocument>.Filter.And(filters))
            //.Limit(1) //Why limit 1 .. we cannot insert multiple with a single id
            .SingleOrDefaultAsync();

        if (doc == null)
        {
            _logger.LogWarning("No document found with Id {id} for entity: {entityType}",
                id, typeof(TEntity).FullName);
            throw new Exception($"AggregateNotFoundException Entity {typeof(TEntity).FullName} " +
                                $"with Id {id} not found");
        }

        var existingKeys = (BsonArray)doc[IdempotencyField] ?? new BsonArray();
        if (!string.IsNullOrWhiteSpace(idempotencyKey))
            foreach (var keyDoc in existingKeys)
            {
                var oneKey = ((BsonString)keyDoc["value"]).Value;
                if (oneKey == idempotencyKey)
                {
                    // operation was already performed
                    _logger.LogDebug("Update operation already performed. So returning");
                    return;
                }
            }

        doc.Remove(IdempotencyField);
        var entity = BsonSerializer.Deserialize<TEntity>(doc);
        var supportOptimisticConcurrency = entity as ISupportOptimisticConcurrency;
        if (supportOptimisticConcurrency != null && version.HasValue)
            if (supportOptimisticConcurrency.Version != version)
            {
                _logger.LogWarning("Entity version mismatch while updating");
                throw new Exception("Entity version mismatch exception");
            }

        await updateAction(entity);

        if (supportOptimisticConcurrency != null)
            //Add version filter as the version might have been modified
            if (version.HasValue)
            {
                //Can be a const...TODO
                var versionFieldName = nameof(ISupportOptimisticConcurrency.Version);
                versionFieldName = char.ToLower(versionFieldName[0]) + versionFieldName.Substring(1);

                //Can we take version from the entity instead of sending as a parameter
                var versionFilter = Builders<BsonDocument>.Filter.Eq(versionFieldName,
                    version.Value);
                filters.Add(versionFilter);
            }

        doc = entity.ToBsonDocument();
        if (!string.IsNullOrWhiteSpace(idempotencyKey))
            existingKeys.Add(CreateIdempotentKeyDocument(idempotencyKey, DateTime.UtcNow));
        // TODO: Remove old idempotency keys
        doc[IdempotencyField] = existingKeys;
        var replaceResult = await GenericCollection
            .ReplaceOneAsync(Builders<BsonDocument>.Filter.And(filters), doc, new ReplaceOptions
            {
                IsUpsert = false
            });

        if (replaceResult.MatchedCount == 1)
        {
            _logger.LogDebug("Updated the document successfully for Id: {@Id}", id);
        }
        else
        {
            _logger.LogError("Error while updating the document. " +
                             "Guess version must have changed in between. MatchedCount: {matchedCount}," +
                             "ReplaceResult: {@replaceResult}",
                replaceResult.MatchedCount, replaceResult);
            throw new ApplicationException("Error while updating the document");
        }
    }

    public async Task<bool> Exists(TId id)
    {
        if (id == null) throw new ArgumentNullException(nameof(id));

        var entityFilter = GetEntityIdFilter(id);

        var count = await GenericCollection.CountDocumentsAsync(entityFilter);

        _logger.LogDebug("Got count {count} when checking for existence of {entity} with id={id}",
            count, typeof(TEntity).Name, id);
        return count != 0;
    }

    public async Task<bool> Exists(Expression<Func<TEntity, bool>> filter)
    {
        var views = GetCollectionAsQueryable();

        if (filter != null) views = views.Where(filter);

        var entities = await views.Select(x => x.Id).ToListAsync();
        return entities.Any();
    }

    public async Task<TEntity> GetById(TId id)
    {
        if (id == null) throw new ArgumentNullException(nameof(id));

        var entityFilter = GetEntityIdFilter(id);
        var doc = await GenericCollection.Find(entityFilter)
            .SingleOrDefaultAsync();

        if (doc != null)
        {
            doc.Remove(IdempotencyField);
            var aggregate = BsonSerializer.Deserialize<TEntity>(doc);
            return aggregate;
        }

        return default;
    }

    public async Task<IReadOnlyCollection<T>> GetAll<T>(Expression<Func<TEntity, bool>>? filter,
        Expression<Func<TEntity, T>> projection,
        IEnumerable<OrderByClause<TEntity>>? orderByClauses,
        int? startIndex = null, int? limit = null)
    {
        ArgumentNullException.ThrowIfNull(projection);

        var views = GetCollectionAsQueryable();

        if (filter != null) views = views.Where(filter);

        if (orderByClauses != null)
        {
            var index = 0;
            foreach (var orderByClause in orderByClauses)
            {
                views = index == 0
                    ? orderByClause.IsDescending
                        ? views.OrderByDescending(orderByClause.PropertySelect)
                        : views.OrderBy(orderByClause.PropertySelect)
                    : orderByClause.IsDescending
                        ? ((IOrderedMongoQueryable<TEntity>?)views).ThenByDescending(orderByClause
                            .PropertySelect)
                        : ((IOrderedMongoQueryable<TEntity>?)views).ThenBy(orderByClause
                            .PropertySelect);
                index++;
            }
        }

        if (startIndex != null) views = views.Skip(startIndex.Value);

        if (limit.HasValue) views = views.Take(limit.Value);

        return await views.Select(projection).ToListAsync();
    }

    public async Task Delete(TId id)
    {
        if (id == null) throw new ArgumentNullException(nameof(id));

        var entityFilter = GetEntityIdFilter(id);
        await GenericCollection.DeleteOneAsync(entityFilter);
    }

    public async Task<TEntity> GetEntityOrDefault(TId id)
    {
        if (id == null) throw new ArgumentNullException(nameof(id));

        var genericCollection = _dbContext.GetGenericCollection<TEntity>();
        var entityFilter = GetEntityIdFilter(id);

        _logger.LogDebug("TEntity Id : {id}", id);
        var doc = await genericCollection.Find(entityFilter).SingleOrDefaultAsync();

        if (doc == null) return default;
        doc.Remove(IdempotencyField);
        var entity = BsonSerializer.Deserialize<TEntity>(doc);
        return entity;
    }

    private BsonDocument CreateIdempotentKeyDocument(string key, DateTime dateTime)
    {
        var doc = new BsonDocument { ["value"] = key, ["date"] = new BsonDateTime(dateTime) };

        return doc;
    }

    private FilterDefinition<BsonDocument> GetEntityIdFilter(TId id)
    {
        var cm = BsonClassMap.LookupClassMap(typeof(TEntity));
        var serializer = cm.IdMemberMap.GetSerializer();
        var bsonValue = serializer.ToBsonValue(id);

        _logger.LogDebug("Filter by id {id} and element is {element}", bsonValue, cm.IdMemberMap.ElementName);

        var entityFilter = Builders<BsonDocument>.Filter.Eq("_id", bsonValue);
        return entityFilter;
    }

    protected virtual IMongoQueryable<TEntity> GetCollectionAsQueryable()
    {
        var collation = new Collation("en");
        var aggregateOptions = new AggregateOptions { Collation = collation };
        return Collection.AsQueryable(aggregateOptions);
    }
}

public interface IEntity<out TId>
{
    public TId Id { get; }
}