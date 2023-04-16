using System.Runtime.Serialization;

namespace mongo_scratch.Infrastructure;

public abstract class AbstractIdentity<TId> : IIdentity<TId>, IComparable
    where TId : IComparable
{
    protected AbstractIdentity(TId value)
    {
        Value = value;
    }

    protected AbstractIdentity(SerializationInfo info, StreamingContext context)
    {
        //TODO?
    }

    public TId Value { get; protected set; }

    public abstract string EntityType { get; }

    public IComparable GetValue()
    {
        return Value;
    }

    public virtual string GetStringValue()
    {
        return $"{Value}";
    }

    public int CompareTo(object obj)
    {
        var other = obj as AbstractIdentity<TId>;
        if (ReferenceEquals(other, null)) return -1;

        return Value.CompareTo(other.Value);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(obj, this)) return true;

        var other = obj as AbstractIdentity<TId>;
        if (ReferenceEquals(other, null)) return false;

        if (other.EntityType != EntityType) return false;

        if (Value.CompareTo(other.Value) != 0) return false;

        return true;
    }

    public override int GetHashCode()
    {
        return EntityType.GetHashCode() * 59 + Value.GetHashCode();
    }

    public override string ToString()
    {
        return $"{EntityType}Id({Value})";
    }
}