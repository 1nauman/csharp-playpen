using mongo_generic_repository.Seedwork;

namespace mongo_generic_repository.Application.Models;

public class EmployeePayrollRecord : IEntity<long>
{
    public long Id { get; init; }

    public long EmployeeId { get; init; }

    public decimal BasicSalary { get; init; }
}