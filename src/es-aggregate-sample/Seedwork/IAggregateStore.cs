namespace es_aggregate_sample.Seedwork;

public interface IAggregateStore
{
    Task Save<T>(T aggregate, CommandMetadata commandMetadata) where T : AggregateRoot;
    
    Task<T> Load<T>(string aggregateId) where T : AggregateRoot;
}