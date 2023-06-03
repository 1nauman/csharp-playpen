using DDD.Sample.Seedwork;

namespace DDD.Sample.EventSourcing;

public interface IAggregateStore
{
    Task Save<T>(T aggregate, CommandMetadata metadata) where T : AggregateRoot;

    Task<T> Load<T>(Guid aggregateId) where T : AggregateRoot;
}