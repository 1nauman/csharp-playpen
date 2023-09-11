public class AwardOption
{
    public AwardOption(int employeeId, int options)
    {
        EmployeeId = employeeId;
        Options = options;
    }

    public int EmployeeId { get; }
    public int Options { get; }
}