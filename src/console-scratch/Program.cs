// See https://aka.ms/new-console-template for more information

var cleanedString =
    "1, 2 ,, ,abc ,9847 ,bnd".Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
Console.WriteLine(string.Join('|', cleanedString));