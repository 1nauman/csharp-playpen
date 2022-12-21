// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using System.Text;
using Amazon;
using Amazon.KeyManagementService;
using AWS.EncryptionSDK;
using AWS.EncryptionSDK.Core;

var stp = Stopwatch.StartNew();
var encryptSdk = AwsEncryptionSdkFactory.CreateDefaultAwsEncryptionSdk();
var encryptMaterialProviders = AwsCryptographicMaterialProvidersFactory.CreateDefaultAwsCryptographicMaterialProviders();

var mrkToEncrypt = "singapore-region-key-arn";

var mrkToDecrypt = "mumbai-region-key-arn";

var mrkKeyringInput = new CreateAwsKmsMrkKeyringInput
{
    KmsClient = new AmazonKeyManagementServiceClient(RegionEndpoint.GetBySystemName(Arn.Parse(mrkToEncrypt).Region)),
    KmsKeyId = mrkToEncrypt
};

var encryptKeyring = encryptMaterialProviders.CreateAwsKmsMrkKeyring(mrkKeyringInput);

var encryptInput = new EncryptInput
{
    Plaintext = new MemoryStream("plaintext"u8.ToArray()),
    Keyring = encryptKeyring
};

var encryptedOutput = encryptSdk.Encrypt(encryptInput);

var base64CipherText = Convert.ToBase64String(encryptedOutput.Ciphertext.ToArray());

Console.WriteLine(base64CipherText);
Console.WriteLine($"Encryption took: {stp.ElapsedMilliseconds} ms");

stp.Restart();

// Decrypt
var decryptSdk = AwsEncryptionSdkFactory.CreateDefaultAwsEncryptionSdk();
var decryptMaterialProviders = AwsCryptographicMaterialProvidersFactory.CreateDefaultAwsCryptographicMaterialProviders();
var mrkDecryptInput = new CreateAwsKmsMrkKeyringInput
{
    KmsClient = new AmazonKeyManagementServiceClient(RegionEndpoint.GetBySystemName(Arn.Parse(mrkToDecrypt).Region)),
    KmsKeyId = mrkToDecrypt
};

var mrkDecryptKeyring = decryptMaterialProviders.CreateAwsKmsMrkKeyring(mrkDecryptInput);

var decryptInput = new DecryptInput
{
    Ciphertext = encryptedOutput.Ciphertext,
    Keyring = mrkDecryptKeyring
};

var decryptOutput = decryptSdk.Decrypt(decryptInput);

Console.WriteLine(Encoding.UTF8.GetString(decryptOutput.Plaintext.ToArray()));

stp.Stop();
Console.WriteLine($"Decryption took: {stp.ElapsedMilliseconds} ms");

