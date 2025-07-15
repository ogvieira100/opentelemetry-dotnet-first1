using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace Api3.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };


        private readonly ILogger<WeatherForecastController> _logger;
        readonly HttpClient _httpClient;
        public WeatherForecastController(IHttpClientFactory httpContextFactory, ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
            _httpClient = httpContextFactory.CreateClient("Api3");
        }

        [HttpGet("Second")]
        public IEnumerable<WeatherForecast> Second()
        {
            _logger.LogInformation("Fetching weather forecast data second Api3");
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            _logger.LogInformation("Fetching weather forecast data Api3");
            await _httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get,
                                 "WeatherForecast/Second"));
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
