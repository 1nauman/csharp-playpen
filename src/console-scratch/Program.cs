// See https://aka.ms/new-console-template for more information

// var cleanedString =
//     "1, 2 ,, ,abc ,9847 ,bnd".Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
// Console.WriteLine(string.Join('|', cleanedString));

using System.Globalization;
using System.Runtime.CompilerServices;
using console_scratch;

var first = new StockPrice[]
{
    new(DateOnly.Parse("2021-01-01"), 100),
    new(DateOnly.Parse("2021-01-03"), 110),
};

// create second array
var second = new StockPrice[]
{
    new(DateOnly.Parse("2021-01-01"), 500),
    new(DateOnly.Parse("2021-01-02"), 110),
};

var union = first.UnionBy(second, o => o.Date);

// print list to console
foreach (var item in union)
{
    Console.WriteLine(item);
}


// var p = new Person("null", 200);
//
// var q = p with { Name = "empty" };
//
// Console.WriteLine(p);
// Console.WriteLine(q);
//
// public record Person(string Name, int Age)
// {
//     private readonly string _name = Name;
//     
//     public string Name
//     {
//         get => _name;
//         init
//         {
//             ArgumentException.ThrowIfNullOrWhiteSpace(value); 
//             _name = value;
//         }
//     }
//
//     public int Age { get; } = Age <= 0 ? throw new ArgumentOutOfRangeException(nameof(Age)) : Age;
// }

// var now = DateTime.UtcNow;
// var anHourAgo = now.AddHours(-1);
// var timeWindow = TimeSpan.FromMinutes(60.0);
//
// var diff = now - anHourAgo;
// Console.WriteLine(diff);
// Console.WriteLine(timeWindow);
// Console.WriteLine(diff >= timeWindow);

// var p1 = new Person
// {
//     Name = "John Doe",
//     Age = 42,
//     Gender = "M"
// };
//
// PrintPerson(p1);
//
// void PrintPerson(in Person person)
// {
//     person.Name = "Jane Doe";
//     Console.WriteLine($"Name: {person.Name}");
//     Console.WriteLine($"Age: {person.Age}");
//     Console.WriteLine($"Gender: {person.Gender}");
// }

//"Test".Sha512();

// GetDateTimeInUtc("2021-09-10", "DateOfBirth");
//
// static string GetDateTimeInUtc(string dateString,
//     [CallerArgumentExpression("dateString")]
//     string memberName = "")
// {
//     if (!DateTime.TryParseExact(dateString, "yyyy-MM-dd", CultureInfo.InvariantCulture,
//             DateTimeStyles.AdjustToUniversal, out var result))
//     {
//         return result.ToString("yyyy-MM-ddTHH:mm:ss.000Z");
//     }
//
//     if (string.IsNullOrWhiteSpace(memberName))
//     {
//         throw new Exception($"Invalid value for date: '{dateString}'");
//     }
//
//     var segments = memberName.Split('.');
//     throw new Exception(
//         $"Invalid value received for '{segments.Last()}': '{dateString}'");
// }