namespace mongo_scratch.Infrastructure;

public interface IIdentity : IComparable
{
    string EntityType { get; }

    IComparable GetValue();

    string GetStringValue();
}

public interface IIdentity<out TId> : IIdentity
    where TId : IComparable
{
    TId Value { get; }
}