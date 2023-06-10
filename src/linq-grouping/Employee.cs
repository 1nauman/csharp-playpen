namespace linq_grouping;

public record Employee(long Id, string Name, DateOnly BirthDate, DateOnly JoiningDate)
{
    public override string ToString()
        => $"{Id}, {Name}, {BirthDate:yyyy-MM-dd}, {JoiningDate:yyyy-MM-dd}";
}