namespace mongo_v3_linq_provider;

public sealed class Person
{
    public Guid Id { get; set; }
    
    public string FirstName { get; set; } = string.Empty;
    
    public string LastName { get; set; } = string.Empty;
    
    public DateTime BirthDate { get; set; }
    
    public int? SomeNumber { get; set; }
    
    public string Email { get; set; } = string.Empty;
    
    public override string ToString() => $"{FirstName} {LastName}, {SomeNumber}, ({BirthDate:yyyy-MM-dd})";
}