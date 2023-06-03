namespace DDD.Sample.Seedwork;

public abstract class AggregateState
{
    public Guid Id { get; set; }

    public long Version { get; set; }
}

public abstract class AggregateTenantState : AggregateState
{
    public Guid IssuerId { get; set; }
}