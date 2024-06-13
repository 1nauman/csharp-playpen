using mongo_generic_repository.Application.Models;
using mongo_generic_repository.Infrastructure.Persistence.Models;

namespace mongo_generic_repository.Infrastructure;

public class EmployeePayrollRecordMapper : IEntityMapper<EmployeePayrollRecord, EmployeePayrollRecordStoreEntity, long>
{
    private readonly IEncryptionService _encryptionService;

    public EmployeePayrollRecordMapper(IEncryptionService encryptionService)
    {
        ArgumentNullException.ThrowIfNull(encryptionService);
        _encryptionService = encryptionService;
    }

    public Task<EmployeePayrollRecordStoreEntity> ToStore(EmployeePayrollRecord entity)
    {
        var storeEntity = new EmployeePayrollRecordStoreEntity
        {
            Id = entity.Id,
            EmployeeId = entity.EmployeeId,
            EncryptedBasicSalary = _encryptionService.Encrypt(entity.BasicSalary) // Encrypt the BasicSalary
        };

        return Task.FromResult(storeEntity);
    }

    public Task<EmployeePayrollRecord> ToEntity(EmployeePayrollRecordStoreEntity storeEntity)
    {
        var entity = new EmployeePayrollRecord
        {
            Id = storeEntity.Id,
            EmployeeId = storeEntity.EmployeeId,
            BasicSalary =
                _encryptionService.Decrypt<decimal>(storeEntity.EncryptedBasicSalary) // Decrypt the BasicSalary
        };

        return Task.FromResult(entity);
    }
}