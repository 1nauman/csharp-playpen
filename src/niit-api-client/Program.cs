// See https://aka.ms/new-console-template for more information

using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using niit_api_client;

// const string url = "https://dv5api.iniitian.com/api/ESOPExt/GetStaffData?Flag={0}";
//
// var services = new ServiceCollection();
// services.AddHttpClient();
// var serviceProvider = services.BuildServiceProvider();
// var httpClient = serviceProvider.GetRequiredService<IHttpClientFactory>().CreateClient();
// var token = GetAuthToken();
// var targetUrl = string.Format(url, token);
// var result = await httpClient.PostAsync(targetUrl, new StringContent(string.Empty));
// Console.WriteLine(await result.Content.ReadAsStringAsync());
// await serviceProvider.DisposeAsync();

var dateTime = new DateTime(2023, 9, 10);

Console.WriteLine(GetAuthToken(dateTime));
Console.WriteLine(BuildToken(dateTime));

static string GetAuthToken(DateTime dateTime)
{
    var dateTimeFormat = "MM/dd/yyyy hh:mm:ss tt";

    var dateCurrent = dateTime.ToString(dateTimeFormat, CultureInfo.InvariantCulture);

    var payLoad = new Dictionary<string, string>
    {
        { "CID", "TalentRepository" },

        { "Time", dateCurrent }
    };

    return JsonWebToken.Encode(payLoad, "superSecretKeyWhichIsMuchLongerThanBefore@345", JwtHashAlgorithm.HS256);
}

static string BuildToken(DateTime dateTime)
{
    var signingKey = "superSecretKeyWhichIsMuchLongerThanBefore@345";
    
    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

    var descriptor = new SecurityTokenDescriptor
    {
        Claims = new Dictionary<string, object>
        {
            { "CID", "TalentRepository" },
            { "Time", dateTime.ToString("MM/dd/yyyy hh:mm:ss tt") }
        },
        SigningCredentials = credentials
    };
    
    var tokenHandler = new JwtSecurityTokenHandler();
    return tokenHandler.WriteToken(tokenHandler.CreateJwtSecurityToken(descriptor));
}