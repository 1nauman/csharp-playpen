// See https://aka.ms/new-console-template for more information

using System.Security.Cryptography;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

var encryptionKey = RSA.Create(3072);  // RSA for encrypting JWE
var signingKey = ECDsa.Create(ECCurve.NamedCurves.nistP256); // ECDSA for signing JWS

var encryptionKeyId = "8b37c2f56cf64c7ebebe1598eb43b8aa";
var signingKeyId = "0bc21824108548a49bc065bae3799b3f";

var privateEncryptionKey = new RsaSecurityKey(encryptionKey) { KeyId = encryptionKeyId };
var publicEncryptionKey = new RsaSecurityKey(encryptionKey.ExportParameters(false)) { KeyId = encryptionKeyId };

var privateSigningKey = new ECDsaSecurityKey(signingKey) { KeyId = signingKeyId };
var publicSigningKey = new ECDsaSecurityKey(ECDsa.Create(signingKey.ExportParameters(false))) { KeyId = signingKeyId };


// Create token
var createTokenHandler = new JsonWebTokenHandler();

var tokenDescriptor = new SecurityTokenDescriptor
{
    Audience = "qapita-scim-api",
    Issuer = "https://myapp.com",
    Claims = new Dictionary<string, object>
    {
        { "sub", "1234567890" },
        { "name", "Numan Mohammed" },
        { "admin", true }
    },
    SigningCredentials = new SigningCredentials(privateSigningKey, SecurityAlgorithms.EcdsaSha256),
    EncryptingCredentials = new EncryptingCredentials(publicEncryptionKey, SecurityAlgorithms.RsaOAEP, SecurityAlgorithms.Aes256CbcHmacSha512)
};

var token = createTokenHandler.CreateToken(tokenDescriptor);

Console.WriteLine(token);

// Decrypt and validate token

var validateTokenHandler = new JsonWebTokenHandler();

var validationResult = validateTokenHandler.ValidateToken(token, new TokenValidationParameters
{
    ValidateIssuer = true,
    ValidIssuer = "https://myapp.com",
    ValidateAudience = true,
    ValidAudience = "qapita-scim-api",
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = publicSigningKey,
    ValidateLifetime = true,
    ValidateTokenReplay = true,
    TokenDecryptionKey = privateEncryptionKey
});

Console.WriteLine(validationResult.IsValid);