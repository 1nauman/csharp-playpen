using Baseline.ImTools;

namespace marten_playpen;

public class Status : IEnumValue
{
    private const string ActiveValue = "Active";
    private const string InactiveValue = "Inactive";

    public static readonly Status Active = new(ActiveValue);
    public static readonly Status Inactive = new(InactiveValue);

    private static readonly Dictionary<string, Status> _allValues =
        new(StringComparer.OrdinalIgnoreCase)
        {
            [ActiveValue] = Active,
            [InactiveValue] = Inactive
        };

    private Status(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Status FromValue(string value)
    {
        if (_allValues.TryGetValue(value, out var status))
        {
            return status;
        }

        throw new ArgumentException($"Unknown status: {value}");
    }

    public override string ToString() => Value;
}