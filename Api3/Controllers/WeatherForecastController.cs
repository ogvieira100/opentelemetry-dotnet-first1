using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Util.Data;
using Util.Dto;
using Util.Models;

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

        readonly IProcessOrder _processOrder;
        readonly ApplicationContext _applicationContext;
        private readonly ILogger<WeatherForecastController> _logger;
        readonly HttpClient _httpClient;
        public WeatherForecastController(ApplicationContext applicationContext,
                 IHttpClientFactory httpContextFactory,
                    IProcessOrder processOrder,
                 ILogger<WeatherForecastController> logger)
        {
            _applicationContext = applicationContext;
            _processOrder = processOrder;
            _logger = logger;
            _httpClient = httpContextFactory.CreateClient("Api1");
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

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CustomerEmployee customer)
        {
            customer =  await _processOrder.ProcessOrderAsync(customer);
            return Ok(customer);
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            _logger.LogInformation("Esse Log mostra as informa��es na instrumenta��o manual Api2");
            _logger.LogError("Esse Log mostra as informa��es na instrumenta��o manual Api2");
            _logger.LogInformation("Fetching weather forecast data Api3");


            Random rand = new Random();
            int numero = rand.Next(0, 100); // Gera n�mero de 0 a 99

            bool ehPar = numero % 2 == 0;

            if (!ehPar)
                throw new Exception("Erro Api3");
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
