namespace mongo_scratch.Infrastructure;

public abstract class StringIdentity : AbstractIdentity<string>
{
    protected StringIdentity(string value) : base(value)
    {
    }

    public static bool operator ==(StringIdentity left, StringIdentity right)
    {
        if (ReferenceEquals(left, right)) return true;

        if (!ReferenceEquals(left, null)) return left.Equals(right);

        return false;
    }

    public static bool operator !=(StringIdentity left, StringIdentity right)
    {
        return !(left == right);
    }

    public override bool Equals(object obj)
    {
        var other = obj as StringIdentity;
        if (ReferenceEquals(other, null)) return false;

        if (!string.Equals(EntityType, other.EntityType)) return false;

        return string.Equals(Value, other.Value);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string GetStringValue()
    {
        return Value;
    }
}