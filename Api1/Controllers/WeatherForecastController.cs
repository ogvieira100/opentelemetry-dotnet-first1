using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using Util.Data;
using Util.Models;

namespace Api1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {


        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        static readonly Meter Meter = new("Meter1");
        static readonly Counter<long> ProdutosCounter = Meter.CreateCounter<long>("produtos_requests_total");

        private readonly ILogger<WeatherForecastController> _logger;
        readonly HttpClient _httpClient;
        readonly ApplicationContext _applicationContext;
        public WeatherForecastController(ApplicationContext applicationContext, IHttpClientFactory httpContextFactory, ILogger<WeatherForecastController> logger)
        {
            _applicationContext = applicationContext;   
            _logger = logger;
            _httpClient = httpContextFactory.CreateClient("Api2");
        }

        [HttpPost("QueePost")]
        public async Task<IActionResult> QueePost([FromBody] Customer customer)
        {
            _logger.LogInformation("Esse Log mostra as informações na instrumentação manual Api1");
            _logger.LogCritical("Esse Log mostra as informações na instrumentação manual Api1 ");
            _logger.LogDebug("Esse Log mostra as informações na instrumentação manual Api1 ");
            _logger.LogError("Esse Log mostra as informações na instrumentação manual Api1 ");
            _logger.LogWarning("Esse Log mostra as informações na instrumentação manual Api1 ");

            Random rand = new Random();
            int numero = rand.Next(0, 100); // Gera número de 0 a 99

            for (int i = 1; i <= 1; i++)
            {
                await Task.Delay(500);
                _logger.LogInformation("Log de teste {i}", i);
                var customerLog = new Customer
                {
                    Nome = $"{customer.Nome} - {i} ",
                    CPF = customer.CPF
                };


                _applicationContext.Add(customerLog);
                await _applicationContext.SaveChangesAsync();

                var request = new HttpRequestMessage(HttpMethod.Post, "QueePost")
                {
                    Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(customerLog), System.Text.Encoding.UTF8, "application/json")
                };
                await _httpClient.SendAsync(request);

            }
            return Ok(customer);

        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Customer customer)
        {
            _logger.LogInformation("Esse Log mostra as informações na instrumentação manual Api1");
            _logger.LogCritical("Esse Log mostra as informações na instrumentação manual Api1 ");
            _logger.LogDebug("Esse Log mostra as informações na instrumentação manual Api1 ");
            _logger.LogError("Esse Log mostra as informações na instrumentação manual Api1 ");
            _logger.LogWarning("Esse Log mostra as informações na instrumentação manual Api1 ");

            Random rand = new Random();
            int numero = rand.Next(0, 100); // Gera número de 0 a 99

            for (int i = 1; i <= 1; i++)
            {
                await Task.Delay(500);
                _logger.LogInformation("Log de teste {i}", i);
                var customerLog = new Customer
                {
                    Nome = $"{customer.Nome} - {i} " ,
                    CPF = customer.CPF
                };


                _applicationContext.Add(customerLog);
                await _applicationContext.SaveChangesAsync();

          

                var request = new HttpRequestMessage(HttpMethod.Post, "WeatherForecast")
                {
                    Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(customerLog), System.Text.Encoding.UTF8, "application/json")
                };  
                await _httpClient.SendAsync(request);

            }
            return Ok(customer);
        }


        [HttpGet("GetAll")]
        public async Task<IEnumerable<Customer>> GetAll()
        {
            _logger.LogInformation("Fetching customer data Api1");
            var customers = await _applicationContext.Set<Customer>()
                .ToListAsync(); 
            return customers;
        }   

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {

            _logger.LogInformation("Esse Log mostra as informações na instrumentação manual Api1");
            _logger.LogError("Esse Log mostra as informações na instrumentação manual Api1");
            ProdutosCounter.Add(1);
            //using var tracerProvider = Sdk.CreateTracerProviderBuilder()
            //    .AddSource("MyCompany.MyProduct.MyLibrary")
            //    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("service-1"))
            //    .AddOtlpExporter(o =>
            //    {
            //        o.Endpoint = new Uri("http://localhost:4317");
            //        o.Protocol = OtlpExportProtocol.Grpc;
            //    })
            //    .Build();

            //var tracer = tracerProvider.GetTracer("MyCompany.MyProduct.MyLibrary");

            //using (var span = tracer.StartActiveSpan("operation"))
            //{
            //    // Simula trabalho
            //    Thread.Sleep(500);
            //}

            _logger.LogInformation("Fetching weather forecast data Api1");
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

        [HttpGet("Second")]
        public IEnumerable<WeatherForecast> Second()
        {
            _logger.LogInformation("Fetching weather forecast data second Api1");
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
