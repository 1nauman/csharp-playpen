// See https://aka.ms/new-console-template for more information

Console.WriteLine("Hello, World!");

var fmvs = Enumerable.Range(0, 30).Select(i => new Fmv { ValuationDate = DateTime.Today.AddDays(-i) });

// foreach(var fmv in fmvs)
//     Console.WriteLine($"{fmv.ValuationDate:s}---{fmv.Id}");

var asOnDate = new DateTime(2022, 12, 23);

var fmvLessThan23 = fmvs.Where(o => o.ValuationDate <= asOnDate).OrderByDescending(o=>o.ValuationDate);

foreach(var fmv in fmvLessThan23)
    Console.WriteLine($"{fmv.ValuationDate:s}---{fmv.Id}");

Console.WriteLine($"First valuation date: {fmvLessThan23.FirstOrDefault().ValuationDate:s}");

public class Fmv
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime ValuationDate { get; set; }
}