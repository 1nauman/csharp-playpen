using System.Collections.Concurrent;

namespace concurrent_dictionary_updates;

public class RateLimiter
{
    private readonly ConcurrentDictionary<string, RateLimitInfo> _rateLimits = new();

    public IReadOnlyDictionary<string, RateLimitInfo> RateLimits => _rateLimits;

    public bool Increment(string key)
    {
        bool updated;
        do
        {
            // Attempt to get existing RateLimitInfo
            var success = _rateLimits.TryGetValue(key, out var currentRateLimitInfo);

            // Prepare a new or incremented RateLimitInfo
            var newRateLimit = success ? 
                currentRateLimitInfo with { Count = currentRateLimitInfo.Count + 1 } : 
                new RateLimitInfo(1, DateTime.UtcNow);

            if (success)
            {
                // Try to update the value atomically
                updated = _rateLimits.TryUpdate(key, newRateLimit, currentRateLimitInfo);
            }
            else
            {
                // Try to add it, and if it exists already; loop to try update
                updated = _rateLimits.TryAdd(key, newRateLimit);
            }
        }
        while (!updated); // Loop until successfully added or updated to handle concurrent updates efficiently

        return true;
    }
}