using Microsoft.AspNetCore.Mvc;

namespace aspnet_serilog_sample.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
        LogMessageWithAllLevels("WeatherForecastController constructor");
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        LogMessageWithAllLevels("GetWeatherForecast called");
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }

    private void LogMessageWithAllLevels(string msg)
    {
        _logger.LogTrace(msg);
        _logger.LogDebug(msg);
        _logger.LogInformation(msg);
        _logger.LogWarning(msg);
        _logger.LogError(msg);
        _logger.LogCritical(msg);
    }
}