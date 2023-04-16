using mongo_scratch.Infrastructure;

namespace mongo_scratch.Models;

public class Employee : IEntity<EmployeeId>
{
    public Employee(EmployeeId employeeId)
    {
        Id = employeeId;
    }
    
    public EmployeeId Id { get; }
    public PersonName Name { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Department { get; set; }
}

public class EmployeeId : StringIdentity
{
    public EmployeeId(string value) : base(value)
    {
    }

    public override string EntityType => "Employee";
}

public class PersonName
{
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
}