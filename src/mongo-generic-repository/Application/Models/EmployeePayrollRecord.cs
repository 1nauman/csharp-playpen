using mongo_generic_repository.Seedwork;

namespace mongo_generic_repository.Application.Models;

public class EmployeePayrollRecord : IEntity<long>
{
    public long Id { get; set; }

    public long EmployeeId { get; set; }

    public decimal BasicSalary { get; set; }
}