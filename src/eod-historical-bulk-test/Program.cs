// See https://aka.ms/new-console-template for more information

using Newtonsoft.Json;

var eodApi = new EOD.API("demo");
var result = eodApi.GetBulksAsync("US", null, null, "MSFT,INFY.NSE,TCS.NSE");

Console.WriteLine(JsonConvert.SerializeObject(result.Result, Formatting.Indented,
    new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));