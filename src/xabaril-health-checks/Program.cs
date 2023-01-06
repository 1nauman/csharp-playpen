using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Health checks
builder.Services.AddHealthChecks()
    .AddCheck("self", () => HealthCheckResult.Healthy())
    .AddMongoDb("mongodb://localhost:27017", mongoDatabaseName: "ct-read-model", tags: new[] { "mongodb", "databases" });

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.UseHealthChecks("/health", new HealthCheckOptions { Predicate = p => p.Name.Contains("self") });
app.UseHealthChecks("/ready", new HealthCheckOptions { Predicate = p => p.Tags.Contains("databases") });
app.UseHealthChecks("/mongo-health", new HealthCheckOptions { Predicate = p => p.Tags.Contains("mongodb") });

app.Run();