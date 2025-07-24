using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http;
using Util.Data;
using Util.Dto;
using Util.Models;

namespace Api2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

      //  private static readonly ActivitySource _activitySource = new("Api2.Weather");

        private readonly ILogger<WeatherForecastController> _logger;
        readonly HttpClient _httpClient;
        readonly ApplicationContext _applicationContext;
        public WeatherForecastController(ApplicationContext applicationContext, IHttpClientFactory httpContextFactory, ILogger<WeatherForecastController> logger)
        {
            _applicationContext = applicationContext;
            _logger = logger;
            _httpClient = httpContextFactory.CreateClient("Api3");
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Customer customer)
        {

            //using var activity = _activitySource.StartActivity("POST WeatherForecast Api2 body", ActivityKind.Server);

            //if (activity != null)
            //{
            //    activity.SetTag("http.request.body", System.Text.Json.JsonSerializer.Serialize(customer));
            //}

            _logger.LogInformation("Esse Log mostra as informações na instrumentação manual Api2");
            _logger.LogCritical("Esse Log mostra as informações na instrumentação manual Api2 ");
            _logger.LogDebug("Esse Log mostra as informações na instrumentação manual Api2 ");
            _logger.LogError("Esse Log mostra as informações na instrumentação manual Api2 ");
            _logger.LogWarning("Esse Log mostra as informações na instrumentação manual Ap2 ");

            Random rand = new Random();
            int numero = rand.Next(0, 100); // Gera número de 0 a 99

            for (int i = 1; i <= 1; i++)
            {
                await Task.Delay(500);
                _logger.LogInformation("Log de teste {i}", i);
                var customerLog = new Employee
                {
                    NomeFantasia = $"{customer.Nome} - {i} ",
                    CNPJ = customer.CPF
                };

                var customerEmployee = new CustomerEmployee
                {
                    Customer = customer,
                    Employee = customerLog
                };  

                _applicationContext.Add(customerLog);
                await _applicationContext.SaveChangesAsync();
                var request = new HttpRequestMessage(HttpMethod.Post, "WeatherForecast")
                {
                    Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(customerEmployee), System.Text.Encoding.UTF8, "application/json")
                };
                await _httpClient.SendAsync(request);

            }
            return Ok(customer);
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {

            _logger.LogInformation("Esse Log mostra as informações na instrumentação manual Api2");
            _logger.LogError("Esse Log mostra as informações na instrumentação manual Api2");
            _logger.LogInformation("Fetching weather forecast data Api2");
           
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("Second")]
        public async Task<IEnumerable<WeatherForecast>> Second()
        {

      
            Random rand = new Random();
            int numero = rand.Next(0, 100); // Gera número de 0 a 99

            bool ehPar = numero % 2 == 0;

            if (!ehPar)
                throw new Exception("Erro Api2");

            _logger.LogInformation("Fetching weather forecast data second Api2");
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
