// See https://aka.ms/new-console-template for more information

// var cleanedString =
//     "1, 2 ,, ,abc ,9847 ,bnd".Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
// Console.WriteLine(string.Join('|', cleanedString));

using System.Globalization;
using System.Runtime.CompilerServices;
using console_scratch;

//"Test".Sha512();

GetDateTimeInUtc("2021-09-10", "DateOfBirth");

static string GetDateTimeInUtc(string dateString,
    [CallerArgumentExpression("dateString")]
    string memberName = "")
{
    if (!DateTime.TryParseExact(dateString, "yyyy-MM-dd", CultureInfo.InvariantCulture,
            DateTimeStyles.AdjustToUniversal, out var result))
    {
        return result.ToString("yyyy-MM-ddTHH:mm:ss.000Z");
    }

    if (string.IsNullOrWhiteSpace(memberName))
    {
        throw new Exception($"Invalid value for date: '{dateString}'");
    }

    var segments = memberName.Split('.');
    throw new Exception(
        $"Invalid value received for '{segments.Last()}': '{dateString}'");
}