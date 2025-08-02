using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Signatures;

namespace MainApp.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IBackgroundJobClient _backgroundJobClient;
    public WeatherForecastController(ILogger<WeatherForecastController> logger, IBackgroundJobClient ackgroundJobClient)
    {
        _logger = logger;
        _backgroundJobClient = ackgroundJobClient;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        _backgroundJobClient.Enqueue<IRecurringJobs>(x => x.TestConcurrentExecutionWithoutCancellation());
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }
}