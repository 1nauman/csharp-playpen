using DDD.Sample.Application.Persistence;
using DDD.Sample.Domain.Issuers;
using DDD.Sample.EventSourcing;

namespace DDD.Sample.Infrastructure.EventStore;

public class EventStoreIssuerRepository : IIssuerRepository
{
    private readonly IAggregateStore _aggregateStore;

    public EventStoreIssuerRepository(IAggregateStore aggregateStore)
    {
        _aggregateStore = aggregateStore;
    }

    public Task<Issuer> Get(Guid issuerId)
    {
        return _aggregateStore.Load<Issuer>(issuerId);
    }

    public Task Save(Issuer issuer, CommandMetadata commandMetadata)
    {
        return _aggregateStore.Save(issuer, commandMetadata);
    }
}