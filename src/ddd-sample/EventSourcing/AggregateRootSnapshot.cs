using DDD.Sample.Seedwork;

namespace DDD.Sample.EventSourcing;

public abstract class AggregateRootSnapshot : AggregateRoot
{
    private Action<object> _snapshotLoad = default!;

    private Func<object> _snapshotGet = default!;

    public long SnapshotVersion { get; private set; }

    protected void RegisterSnapshot<T>(Action<T> load, Func<object> get)
    {
        _snapshotLoad = e => load((T)e);
        _snapshotGet = get;
    }

    public void LoadSnapshot(object snapshot, long version)
    {
        _snapshotLoad(snapshot);
        Version = version;
        SnapshotVersion = version;
    }

    public object GetSnapshot() =>
        _snapshotGet();
}