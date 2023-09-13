using Microsoft.AspNetCore.Mvc;
using Back_end.Models;

namespace Back_end.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly NotificationContext context1;
    private readonly User user1;
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, NotificationContext context)
    {
        context1 = context;
        _logger = logger;
        User? user = context1.Users.Find(1);
        user1 = user;
    }



    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            // Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            Summary = user1.Email
        })
        .ToArray();
    }
}
