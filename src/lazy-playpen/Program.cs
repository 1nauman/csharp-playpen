// See https://aka.ms/new-console-template for more information

using System.Collections.Concurrent;
using Dumpify;

Console.WriteLine("Hello, World!");

var limits = new ConcurrentDictionary<string, Lazy<RateLimitInfo>>();

var tasks = Enumerable.Range(0, 100).Select(i =>
{
    return Task.Run(() => limits.AddOrUpdate($"key",
        _ => new Lazy<RateLimitInfo>(() => new RateLimitInfo
        {
            Counter = 1,
            StartTime = DateTime.UtcNow
        }), (s, info) =>
        {
            info.Value.Counter++;
            return info;
        }));
}).ToArray();

await Task.WhenAll(tasks);

limits.Dump();

public class RateLimitInfo
{
    public int Counter { get; set; }

    public DateTime StartTime { get; set; }
}