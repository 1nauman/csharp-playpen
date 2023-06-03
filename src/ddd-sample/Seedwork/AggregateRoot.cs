namespace DDD.Sample.Seedwork;

public abstract class AggregateRoot
{
    private readonly Dictionary<Type, Action<object>> _handlers = new();
    private readonly List<object> _changes = new();
    
    public Guid Id { get; set; } = default;
    
    protected abstract AggregateState State { get; }
    
    public long Version { get; protected set; } = -1;

    protected void Register<T>(Action<T> when) =>
        _handlers.Add(typeof(T), e => when((T) e));

    protected void Raise(object e)
    {
        _handlers.TryGetValue(e.GetType(), out var handler);

        if (handler == null)
        {
            throw new NoHandlerRegisteredException(e.GetType());
        }

        handler(e);
        _changes.Add(e);
    }

    public IEnumerable<object> GetChanges() => _changes.AsEnumerable();

    public void Load(IEnumerable<object> history)
    {
        foreach (var e in history)
        {
            Raise(e);
            Version++;
        }
    }

    public void ClearChanges() => _changes.Clear();
}

public abstract class AggregateRoot<TState> : AggregateRoot where TState : AggregateState, new()
{
    protected override TState State { get; }

    protected AggregateRoot()
    {
        State = new TState();
    }

    protected AggregateRoot(TState state)
    {
        State = state;
    }
}

public class NoHandlerRegisteredException : Exception
{
    public NoHandlerRegisteredException(Type type) : base($"No handler registered for {type.FullName}")
    {
    }
}