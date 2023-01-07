namespace es_aggregate_sample.Seedwork;

public abstract class AggregateRoot
{
    private readonly List<IDomainEvent> _changes = new();

    private readonly Dictionary<Type, Action<IDomainEvent>> _handlers = new();

    public string Id { get; set; } = default!;

    public int Version { get; protected set; } = -1;

    protected void Register<T>(Action<T> handler) where T : IDomainEvent =>
        _handlers.Add(typeof(T), @event => handler((T)@event));

    protected void Raise(IDomainEvent domainEvent)
    {
        _handlers.TryGetValue(domainEvent.GetType(), out var handler);

        if (handler == null) throw new EventHandlerNotRegisteredException(domainEvent.GetType());
        
        handler(domainEvent);
        _changes.Add(domainEvent);
    }
    
    public IEnumerable<IDomainEvent> GetChanges() => _changes.AsEnumerable();
    
    public void ClearChanges() => _changes.Clear();
    
    public void Load(IEnumerable<IDomainEvent> history)
    {
        foreach (var e in history)
        {
            Raise(e);
            Version++;
        }
    }
}

public class EventHandlerNotRegisteredException : Exception
{
    public EventHandlerNotRegisteredException(Type type) : base(
        $"Event handler not registered for event type {type.FullName}")
    {
    }
}