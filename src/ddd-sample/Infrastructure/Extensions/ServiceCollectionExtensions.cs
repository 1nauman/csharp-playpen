using DDD.Sample.Application.Persistence;
using DDD.Sample.Domain.Events;
using DDD.Sample.EventSourcing;
using DDD.Sample.Infrastructure.EventStore;
using EventStore.Client;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        var esDbClient = new EventStoreClient(EventStoreClientSettings.Create("esdb://localhost:2113"));
        var eventStore = new EsDbEventStore(esDbClient, "tenant_");

        var aggregateStore = new EsDbAggregateStore(eventStore, 5);
        services.AddSingleton<IAggregateStore>(aggregateStore);
        services.AddScoped<IIssuerRepository, EventStoreIssuerRepository>();

        EventMappings.MapEventTypes();
        
        return services;
    }
}