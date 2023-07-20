using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace console_scratch;

public static class HashExtensions
{
    public static string Sha512(this string input, bool upperCaseHashResult = false)
    {
        if (string.IsNullOrWhiteSpace(input)) return string.Empty;

        var bytes = Encoding.UTF8.GetBytes(input);
        var hash = SHA512.HashData(bytes);

        string result;

        var stp = Stopwatch.StartNew();
        result = BitConverter.ToString(hash).Replace("-", "").ToLower();
        stp.Stop();
        
        Console.WriteLine($"{stp.Elapsed.Nanoseconds}: {result}");
        
        stp.Restart();
        var stringBuilder = new StringBuilder();
        foreach (var b in hash)
        {
            stringBuilder.Append(b.ToString("x"));
        }
        result = stringBuilder.ToString();
        stp.Stop();
        Console.WriteLine($"{stp.Elapsed.Nanoseconds}: {result}");

        return result;
    }
}