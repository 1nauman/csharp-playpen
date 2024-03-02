// See https://aka.ms/new-console-template for more information

using concurrent_dictionary_updates;
using Dumpify;

TestRateLimiter();
return;

static void TestRateLimiter()
{
    // Instance of the rate limiter
    var rateLimiter = new RateLimiter();
    const string testKey = "user1";
    const int numberOfRequests = 1000; // Number of concurrent requests to simulate

    // Running the increment operation concurrently
    Parallel.For(0, numberOfRequests, (i) => {
        rateLimiter.Increment(testKey);
    });

    // Fetch the RateLimitInfo for the testKey
    var success = rateLimiter.RateLimits.TryGetValue(testKey, out var rateLimitInfo);
        
    // Checking the results
    if (success)
    {
        Console.WriteLine($"Expected count: {numberOfRequests}, Actual count: {rateLimitInfo.Count}");
        Console.WriteLine(rateLimitInfo.Count == numberOfRequests ? "Test Passed" : "Test Failed");
    }
    else
    {
        Console.WriteLine("Test Failed: Key not found");
    }

    rateLimiter.RateLimits.Dump();
}