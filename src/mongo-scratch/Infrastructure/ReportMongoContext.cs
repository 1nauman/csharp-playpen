using Microsoft.Extensions.Logging;
using mongo_scratch.Models;
using MongoDB.Bson.Serialization;

namespace mongo_scratch.Infrastructure;

public class ReportMongoContext : MongoContext, IReportMongoContext
{
    //TODO: Change this later when moved to different project

    private static readonly object LockObj = new();
    private readonly ILogger<ReportMongoContext> _logger;

    public ReportMongoContext(IReportModelDBSettings settings,
        ILogger<ReportMongoContext> logger)
        : base(settings)
    {
        _logger = logger;
    }

    protected override string ConventionName => "Qapita.Capitalization.Application.Reports";

    protected override Func<Type, bool> FilterForObjectsSatisfyingConvention =>
        t => t.FullName.StartsWith("mongo_scratch.Models");

    protected override (Type modelType, string)[] CollectionTypeNameMap
    {
        get
        {
            return new[]
            {
                (typeof(Employee), "employees")
            };
        }
    }

    protected override void RegisterClassMap<T>(Action<BsonClassMap<T>> map)
    {
        _logger.LogDebug("Registering class map for type {type}", typeof(T));
        if (!BsonClassMap.IsClassMapRegistered(typeof(T)))
            lock (LockObj)
            {
                if (!BsonClassMap.IsClassMapRegistered(typeof(T)))
                    base.RegisterClassMap(map);
                else
                    _logger.LogWarning(
                        "Multiple ClassMap registrations for same type {type}. " +
                        "Typically happens in unit test cases", typeof(T).FullName);
            }
        else
            _logger.LogWarning("Multiple ClassMap registrations for same type {type}. " +
                               "Typically happens in unit test cases", typeof(T).FullName);
    }

    protected override void RegisterSerializers()
    {
        BsonSerializer.RegisterSerializationProvider(new CustomSerializationProvider());
    }

    protected override void RegisterIndexes()
    {
    }

    protected override void RegisterClassMaps()
    {
        _logger.LogDebug("Registering Class Maps");
    }
}