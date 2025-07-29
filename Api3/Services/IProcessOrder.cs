using Util.Data;
using Util.Dto;
using Util.Models;

public interface IProcessOrder
{
    Task<CustomerEmployee> ProcessOrderAsync(CustomerEmployee customerEmployee);
}

public class ProcessOrder : IProcessOrder
{
    private readonly ILogger<ProcessOrder> _logger;
    private readonly ApplicationContext _applicationContext;
    private readonly HttpClient _httpClient;

    public ProcessOrder(ILogger<ProcessOrder> logger,
     ApplicationContext applicationContext,
     HttpClient httpClient)
    {
        _logger = logger;
        _applicationContext = applicationContext;
        _httpClient = httpClient;
    }
    public async Task<CustomerEmployee> ProcessOrderAsync(CustomerEmployee customerEmployee)
    {
        _logger.LogInformation("Esse Log mostra as informações na instrumentação manual Api3");
        _logger.LogCritical("Esse Log mostra as informações na instrumentação manual Api3 ");
        _logger.LogDebug("Esse Log mostra as informações na instrumentação manual Api3 ");
        _logger.LogError("Esse Log mostra as informações na instrumentação manual Api3 ");
        _logger.LogWarning("Esse Log mostra as informações na instrumentação manual Api3 ");

        Random rand = new Random();
        int numero = rand.Next(0, 100); // Gera n�mero de 0 a 99

        for (int i = 1; i <= 2; i++)
        {
            await Task.Delay(500);
            _logger.LogInformation("Log de teste {i}", i);

            var order = new Order
            {
                CustomerId = customerEmployee.Customer.Id,
                EmployeeId = customerEmployee.Employee.Id,
                Description = $"Pedido {i} do cliente {customerEmployee.Customer.Nome} para o funcionário {customerEmployee.Employee.NomeFantasia}"
            };
            _applicationContext.Add(order);
            await _applicationContext.SaveChangesAsync();
        }
        await _httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get,
                              "WeatherForecast/GetAll"));

        return customerEmployee;
    }
}   