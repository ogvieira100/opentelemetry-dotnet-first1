using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Diagnostics.Metrics;
using Util.Data;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Logging.AddOpenTelemetry(options =>
{
    options.IncludeFormattedMessage = true;
    options.IncludeScopes = true;
    options.ParseStateValues = true;
  
});


builder.Services.AddOpenTelemetry()
    .ConfigureResource(r => r
    .AddService(
            serviceName: "service-1",
            serviceVersion: typeof(Program).Assembly.GetName().Version?.ToString() ?? "unknown",
            serviceInstanceId: Environment.MachineName))
     .WithTracing(tracing =>
     {
         tracing
             .AddSqlClientInstrumentation(options =>
             {
                 options.SetDbStatementForText = true;
             })
             .AddAspNetCoreInstrumentation(options =>
            {
                options.RecordException = true;
                options.EnrichWithException = (activity, exception) =>
                {
                    activity.SetTag("exception.type", exception.GetType().Name);
                    activity.SetTag("exception.message", exception.Message);
                    activity.SetTag("exception.stacktrace", exception.StackTrace);
                };
                options.EnrichWithHttpResponse = (activity, httpResponse) =>
                {
                    activity.SetTag("http.status_code", httpResponse.StatusCode);
                    activity.SetTag("http.response_content_length", httpResponse.ContentLength);
                };
                options.EnrichWithHttpRequest = (activity, httprequest) =>
                {
                    activity.SetTag("http.request_content_length", httprequest.ContentLength);
                    activity.SetTag("http.user_agent", httprequest.Headers.UserAgent.ToString());
                    activity.SetTag("http.custom_header", httprequest.Headers["X-Correlation-Id"].ToString());
                };
            })
             .AddHttpClientInstrumentation()
             .AddOtlpExporter(otlpOptions =>
             {
                 otlpOptions.Endpoint = new Uri(builder.Configuration.GetValue("Otlp:Endpoint", "http://localhost:4317")!);
             });
     })
    .WithLogging(logging =>
    {
       
        logging.AddOtlpExporter(otlpOptions =>
        {
            // Use IConfiguration directly for Otlp exporter endpoint option.
            otlpOptions.Endpoint = new Uri(builder.Configuration.GetValue("Otlp:Endpoint", defaultValue: "http://localhost:4317")!);
        });
    })
  .WithMetrics(metric =>
  {

      metric
       .AddMeter("Meter1")
       .AddRuntimeInstrumentation()
       .AddHttpClientInstrumentation()
       .AddAspNetCoreInstrumentation()
       .AddPrometheusExporter(); // 👈 AQUI
      ;
      metric.AddOtlpExporter(otlpOptions =>
      {
          // Use IConfiguration directly for Otlp exporter endpoint option.
          otlpOptions.Endpoint = new Uri(builder.Configuration.GetValue("Otlp:Endpoint", defaultValue: "http://localhost:4317")!);
          otlpOptions.Protocol = OtlpExportProtocol.Grpc;
      });
  });

builder.Services.AddHttpClient("Api2", client =>
{

    client.BaseAddress = new Uri(builder.Configuration.GetSection("Api2:Base").Value!);

});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationContext>(options =>
                 options.UseSqlServer(connectionString)
                .EnableSensitiveDataLogging()
                .UseLazyLoadingProxies()
    );

builder.Services.AddScoped<ApplicationContext>();
var app = builder.Build();


app.UseOpenTelemetryPrometheusScrapingEndpoint(); // para expor o /metrics


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
