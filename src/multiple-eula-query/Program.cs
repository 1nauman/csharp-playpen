// See https://aka.ms/new-console-template for more information

//Console.WriteLine("Hello, World!");

using multiple_eula_query;

var eulas = new[]
{
    new Eula(1, new DateTime(2010, 01, 01), string.Empty),
    new Eula(2, new DateTime(2011, 01, 01), string.Empty, 1, false),
    new Eula(3, new DateTime(2012, 01, 01), string.Empty, 2),
    new Eula(4, new DateTime(2022, 01, 01), string.Empty, 1, false),
    new Eula(50, new DateTime(2023, 01, 01), string.Empty, 1)
};

var issuerEulas = eulas.Where(o => o.IssuerId == null || o.IssuerId == 1);

foreach (var e in issuerEulas)
{
    Console.WriteLine($"{e.Id}--{e.CreatedOn:s}--{e.IssuerId}");
}

var latestEula = issuerEulas.MaxBy(o => o.CreatedOn);

Console.WriteLine($"Latest Eula: {latestEula.Id}--{latestEula.CreatedOn:s}--{latestEula.IssuerId}");