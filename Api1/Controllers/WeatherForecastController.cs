using Microsoft.AspNetCore.Mvc;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

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

        private readonly ILogger<WeatherForecastController> _logger;
        readonly HttpClient _httpClient;
        public WeatherForecastController(IHttpClientFactory httpContextFactory, ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
            _httpClient = httpContextFactory.CreateClient("Api2");
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
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
