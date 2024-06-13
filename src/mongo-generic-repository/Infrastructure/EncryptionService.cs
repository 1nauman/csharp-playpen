using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace mongo_generic_repository.Infrastructure;

public interface IEncryptionService
{
    string Encrypt<T>(T value);

    T? Decrypt<T>(string? encryptedValue);
}

public class EncryptionService : IEncryptionService
{
    private readonly byte[] _encryptionKey = "YourSecretKey123"u8.ToArray(); // Replace with a secure key

    public string Encrypt<T>(T value)
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

    public T? Decrypt<T>(string? encryptedValue)
    {
        if (encryptedValue == null)
        {
            return default;
        }

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