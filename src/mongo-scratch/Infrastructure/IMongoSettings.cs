namespace mongo_scratch.Infrastructure;

public interface IMongoSettings
{
    string ConnectionString { get; }

    string DatabaseName { get; }
}