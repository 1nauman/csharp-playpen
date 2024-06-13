using System.Security.Cryptography;
using System.Text;
using mongo_generic_repository.Application.Models;
using mongo_generic_repository.Infrastructure.Persistence.Models;
using Newtonsoft.Json;

namespace mongo_generic_repository.Infrastructure;

public class EmployeePayrollRecordMapper : IEntityMapper<EmployeePayrollRecord, EmployeePayrollRecordStoreEntity, long>
{
    private readonly byte[] _encryptionKey = "YourSecretKey123"u8.ToArray(); // Replace with a secure key

    public Task<EmployeePayrollRecordStoreEntity> ToStore(EmployeePayrollRecord entity)
    {
        var storeEntity = new EmployeePayrollRecordStoreEntity
        {
            Id = entity.Id,
            EmployeeId = entity.EmployeeId,
            EncryptedBasicSalary = Encrypt(entity.BasicSalary) // Encrypt the BasicSalary
        };

        return Task.FromResult(storeEntity);
    }

    public Task<EmployeePayrollRecord> ToEntity(EmployeePayrollRecordStoreEntity storeEntity)
    {
        var entity = new EmployeePayrollRecord
        {
            Id = storeEntity.Id,
            EmployeeId = storeEntity.EmployeeId,
            BasicSalary = Decrypt<decimal>(storeEntity.EncryptedBasicSalary) // Decrypt the BasicSalary
        };

        return Task.FromResult(entity);
    }

    private string Encrypt<T>(T value)
    {
        using var aesAlg = Aes.Create();
        aesAlg.Key = _encryptionKey;
        aesAlg.IV = _encryptionKey; // For demonstration purposes only. Use a secure IV in production.

        // Serialize the value to JSON (or any suitable format)
        var serializedValue = JsonConvert.SerializeObject(value); 
        var plainTextBytes = Encoding.UTF8.GetBytes(serializedValue);

        var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
        using var msEncrypt = new MemoryStream();
        using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
        {
            csEncrypt.Write(plainTextBytes, 0, plainTextBytes.Length);
        }
        return Convert.ToBase64String(msEncrypt.ToArray());
    }

    private T? Decrypt<T>(string? encryptedValue)
    {
        using var aesAlg = Aes.Create();
        aesAlg.Key = _encryptionKey;
        aesAlg.IV = _encryptionKey; // For demonstration purposes only. Use a secure IV in production.

        var cipherTextBytes = Convert.FromBase64String(encryptedValue);
        var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

        using var msDecrypt = new MemoryStream(cipherTextBytes);
        using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
        using var srDecrypt = new StreamReader(csDecrypt);
        var serializedValue = srDecrypt.ReadToEnd();
        return JsonConvert.DeserializeObject<T>(serializedValue); // Deserialize to the original type
    }
}