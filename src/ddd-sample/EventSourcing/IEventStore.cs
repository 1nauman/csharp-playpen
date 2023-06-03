namespace DDD.Sample.EventSourcing;

public interface IEventStore
{
    Task AppendCommand(string streamId, object command, CommandMetadata metadata);

    Task AppendEvents(string streamName, long version, CommandMetadata metadata, params object[] events);

    Task AppendEvents(string streamName, CommandMetadata metadata, params object[] events);

    Task<IEnumerable<object>> LoadEvents(string stream, long? version = null);

    Task<ulong?> GetLastVersion(string streamName);

    Task AppendSnapshot(string streamName, long aggregateVersion, object snapshot);

    Task<SnapshotEnvelope?> LoadSnapshot(string streamName);

    Task TruncateStream(string streamName, ulong version);

    Task<IEnumerable<CommandEnvelope>> LoadCommands(string commandStream);
}

public class CommandEnvelope
{
    public object Command { get; }

    public CommandMetadata Metadata { get; }

    public CommandEnvelope(object command, CommandMetadata metadata)
    {
        Command = command;
        Metadata = metadata;
    }
}

public record SnapshotEnvelope(
    object Snapshot,
    SnapshotMetadata Metadata
);

public record SnapshotMetadata(
    long Version
);