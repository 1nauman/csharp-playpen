using mongo_generic_repository.Seedwork;

namespace mongo_generic_repository.Infrastructure.Persistence.Models;

public class EmployeePayrollRecordStoreEntity : IEntity<long>
{
    public long Id { get; set; }
    public long EmployeeId { get; set; }
    public string? EncryptedBasicSalary { get; set; } // Now a string to hold the encrypted value
}