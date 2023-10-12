// See https://aka.ms/new-console-template for more information

using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using niit_api_client;

const string url = "https://dv5api.iniitian.com/api/ESOPExt/GetStaffData?Flag={0}";
const string key = ""; // Guess this is the signing key for JWT

var services = new ServiceCollection();
services.AddHttpClient();
var serviceProvider = services.BuildServiceProvider();
var httpClient = serviceProvider.GetRequiredService<IHttpClientFactory>().CreateClient();
var token = GetAuthToken(DateTime.Now);
//Console.WriteLine(token);
var targetUrl = string.Format(url, token);

var response = await httpClient.PostAsync(targetUrl, new StringContent(string.Empty));
var resultString = await response.Content.ReadAsStringAsync();
//Console.WriteLine(resultString);

try
{
    if (string.IsNullOrWhiteSpace(resultString))
    {
        Console.WriteLine("Empty result string");
        return;
    }

    if (resultString == "\"\"")
    {
        Console.WriteLine("Empty \"\" result string");
        return;
    }
    
    var resultJson = JsonWebToken.Decode(resultString, key);
    Console.WriteLine(resultJson);
}
finally
{
    await serviceProvider.DisposeAsync();
}

static string GetAuthToken(DateTime dateTime)
{
    const string dateTimeFormat = "MM/dd/yyyy hh:mm:ss tt";

    var dateCurrent = dateTime.ToString(dateTimeFormat, CultureInfo.InvariantCulture);

    var payLoad = new Dictionary<string, string>
    {
        { "CID", "TalentRepository" },
        { "Time", dateCurrent }, // Time at which the token is generated
        { "_Date", "20 Mar 2023" } // Date for which we want to get the data for
    };

    return JsonWebToken.Encode(payLoad, key, JwtHashAlgorithm.HS256);
}