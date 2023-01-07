namespace es_aggregate_sample.Seedwork;

public record CommandMetadata(CorrelationId CorrelationId, CausationId CausationId);

public record CorrelationId(Guid Value);

public record CausationId(Guid Value);