namespace DDD.Sample.EventSourcing;

public record CommandMetadata(
    CorrelationId CorrelationId,
    CausationId CausationId
);

public record CorrelationId(
    Guid Value
);

public record CausationId(
    Guid Value
);