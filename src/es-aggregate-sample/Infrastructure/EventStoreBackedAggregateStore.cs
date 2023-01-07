using es_aggregate_sample.Seedwork;

namespace es_aggregate_sample.Infrastructure;

public class EventStoreBackedAggregateStore : IAggregateStore
{
    private readonly IEventStore _store;
    
    public Task Save<T>(T aggregate, CommandMetadata commandMetadata) where T : AggregateRoot
    {
        throw new NotImplementedException();
    }

    public Task<T> Load<T>(string aggregateId) where T : AggregateRoot
    {
        throw new NotImplementedException();
    }
}