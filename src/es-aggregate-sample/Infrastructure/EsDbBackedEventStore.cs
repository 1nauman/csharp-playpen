using es_aggregate_sample.Seedwork;
using EventStore.Client;

namespace es_aggregate_sample.Infrastructure;

public class EsDbBackedEventStore : IEventStore
{
    private readonly EventStoreClient _client;
    
    public EsDbBackedEventStore(EventStoreClient client)
    {
        _client = client;
    }
}