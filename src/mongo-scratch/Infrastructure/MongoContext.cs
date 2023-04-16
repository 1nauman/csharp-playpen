using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;

namespace mongo_scratch.Infrastructure;

public abstract class MongoContext : IDisposable
{
    private static readonly object LockObj = new();

    //private readonly List<Func<Task>> _commands;
    private readonly IMongoSettings _settings;

    private Dictionary<Type, string> _collectionTypeNameMap;

    //private readonly ILogger<MongoContext> _logger;

    protected MongoContext(IMongoSettings settings)
    {
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        //_logger = logger ?? throw new ArgumentNullException(nameof(logger));

        _collectionTypeNameMap = null;
    }

    private IMongoDatabase Database { get; set; }

    //public IClientSessionHandle Session { get; set; }
    private MongoClient MongoClient { get; set; }

    protected abstract string ConventionName { get; }

    protected abstract Func<Type, bool> FilterForObjectsSatisfyingConvention { get; }

    protected abstract (Type modelType, string)[] CollectionTypeNameMap { get; }

    protected virtual IConvention[] Conventions
    {
        get { return new IConvention[] { new CamelCaseElementNameConvention() }; }
    }

    public void Dispose()
    {
        //Session?.Dispose();
        //GC.SuppressFinalize(this);
    }

    protected virtual void RegisterConventions()
    {
        var pack = new ConventionPack();
        pack.AddRange(Conventions);
        ConventionRegistry.Register(ConventionName, pack, FilterForObjectsSatisfyingConvention);
    }

    protected virtual void RegisterClassMaps()
    {
    }

    protected virtual void RegisterIndexes()
    {
    }

    protected void CreateIndex<T>(params Index<T>[] indices)
    {
        if (Database == null)
            throw new Exception("Call this after configuring Mongo or " +
                                "in Register index method");

        if (indices == null || indices.Length == 0) return;
        var indexBuilder = Builders<T>.IndexKeys;
        IndexKeysDefinition<T> indexKeysDefinition = null;
        foreach (var index in indices)
            if (index.IsAscending)
            {
                if (index.Property != null)
                    indexKeysDefinition = indexKeysDefinition?.Ascending(index.Property)
                                          ?? indexBuilder.Ascending(index.Property);
                else if (index.PropertyName != null)
                    indexKeysDefinition = indexKeysDefinition?.Ascending(index.PropertyName)
                                          ?? indexBuilder.Ascending(index.PropertyName);
            }
            else
            {
                if (index.Property != null)
                    indexKeysDefinition = indexKeysDefinition?.Descending(index.Property)
                                          ?? indexBuilder.Descending(index.Property);
                else if (index.PropertyName != null)
                    indexKeysDefinition = indexKeysDefinition?.Descending(index.PropertyName)
                                          ?? indexBuilder.Descending(index.PropertyName);
            }

        //Not calling GetCollection as it might call this method in recursive. 
        //After moving the configure method to Init call GetCollection<T> here..
        Database.GetCollection<T>(_collectionTypeNameMap[typeof(T)]).Indexes
            .CreateOneAsync(new CreateIndexModel<T>(indexKeysDefinition));
    }

    protected virtual void RegisterClassMap<T>(Action<BsonClassMap<T>> map)
    {
        BsonClassMap.RegisterClassMap(map);
    }

    private void RegisterCollections()
    {
        _collectionTypeNameMap = CollectionTypeNameMap
            .ToDictionary(x => x.Item1, x => x.Item2);
    }

    /// <summary>
    ///     TODO: Move this from get collection method.. and to an init method
    /// </summary>
    private void ConfigureMongo()
    {
        if (MongoClient != null)
            return;
        lock (LockObj)
        {
            if (MongoClient != null) return;
            RegisterConventions();
            RegisterCollections();
            RegisterSerializers();
            RegisterClassMaps();
            var settings = MongoClientSettings.FromConnectionString(_settings.ConnectionString);
            settings.ClusterConfigurator += builder =>
            {
                builder.Subscribe<CommandStartedEvent>(OnCommandStarted);
                builder.Subscribe<CommandSucceededEvent>(OnCommandSucceeded);
            };
            // Configure mongo (You can inject the config, just to simplify)
            MongoClient = new MongoClient(settings);
            Database = MongoClient.GetDatabase(_settings.DatabaseName);
            RegisterIndexes();
        }
    }

    protected virtual void OnCommandSucceeded(CommandSucceededEvent obj)
    {
    }

    protected virtual void OnCommandStarted(CommandStartedEvent obj)
    {
    }

    protected virtual void RegisterSerializers()
    {
    }

    public IMongoCollection<T> GetCollection<T>()
    {
        ConfigureMongo();
        if (_collectionTypeNameMap == null)
            throw new Exception(
                "Please call register collections on Context to register collections. No collections registered");

        if (!_collectionTypeNameMap.ContainsKey(typeof(T)))
            throw new Exception($"No collection defined for type {typeof(T)}");

        return Database.GetCollection<T>(_collectionTypeNameMap[typeof(T)]);
    }

    public IMongoCollection<BsonDocument> GetGenericCollection<T>()
    {
        ConfigureMongo();
        if (_collectionTypeNameMap == null)
            throw new Exception(
                "Please call register collections on Context to register collections. No collections registered");

        if (_collectionTypeNameMap.TryGetValue(typeof(T), out var collectionName))
            return Database.GetCollection<BsonDocument>(collectionName);

        throw new InvalidOperationException($"collection name not registered for type {typeof(T).FullName}");
    }

    public class Index<T>
    {
        public Index(Expression<Func<T, object>> property, bool isAscending)
        {
            Property = property ?? throw new ArgumentNullException(nameof(property));
            IsAscending = isAscending;
        }

        public Index(string propertyName, bool isAscending)
        {
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentException("Value cannot be null or empty.",
                    nameof(propertyName));
            PropertyName = propertyName;
            IsAscending = isAscending;
        }

        public string PropertyName { get; }
        public bool IsAscending { get; }

        public Expression<Func<T, object>> Property { get; }
    }

    // public void AddCommand(Func<Task> func)
    // {
    //     _commands.Add(func);
    // }   
}