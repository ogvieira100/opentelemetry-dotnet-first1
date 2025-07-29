
using Util.Dto;
using Util.MessageBus;
using Util.MessageBus.Models;
using Util.Models;

namespace Api3.Services
{
    public class ProcessOrderHandler : BackgroundService
    {
        readonly IServiceProvider _serviceProvider;
        readonly IMessageBusRabbitMq _messageBusRabbitMq;
        public ProcessOrderHandler(IServiceProvider serviceProvider,
         IMessageBusRabbitMq messageBusRabbitMq)
        {
            _serviceProvider = serviceProvider;
            _messageBusRabbitMq = messageBusRabbitMq;
        }

        
        void SetResponder()
        {
            _messageBusRabbitMq.SubscribeAsync<InsertOrderIntegrationEvent>(
                new PropsMessageQueeDto { Queue = "QueeOrderInsert" },
                     InsertOrdersAsync
                );
        }

        async Task InsertOrdersAsync(InsertOrderIntegrationEvent request)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {

                    var processOrder = scope.ServiceProvider.GetRequiredService<IProcessOrder>();
                    var customerEmployee = new CustomerEmployee
                    {
                        Customer = request.Customer,
                        Employee = request.Employee
                    };
                    await processOrder.ProcessOrderAsync(customerEmployee);
                    
                }
            }
            catch (Exception ex) { }
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
             SetResponder();
             return Task.CompletedTask;
        }
    }
}
