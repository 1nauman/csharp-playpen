namespace mongo_scratch.Infrastructure;

public interface ISupportOptimisticConcurrency
{
    long Version { get; }
}