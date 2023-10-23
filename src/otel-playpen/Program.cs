using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using otel_playpen.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Build a resource configuration action to set service information.
Action<ResourceBuilder> configureResource = r => r.AddService(
    serviceName: "otel-playpen",
    serviceVersion: "1.0",
    serviceNamespace: "otel.playpen",
    serviceInstanceId: Environment.MachineName);

// Add OTel
builder.Services.AddOTel(configureResource);

builder.Logging.ClearProviders();
builder.Logging.AddOpenTelemetry(options =>
{
    var resBuilder = ResourceBuilder.CreateDefault();
    configureResource(resBuilder);
    options.SetResourceBuilder(resBuilder);
    
    options.AddConsoleExporter();
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}