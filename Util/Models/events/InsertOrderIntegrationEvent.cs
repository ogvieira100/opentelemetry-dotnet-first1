using Util.MessageBus.Models.Integration;

namespace Util.Models
{
    public class InsertOrderIntegrationEvent : IntegrationEvent
    {
        public Customer Customer { get; set; }
        public Employee Employee { get; set; }
    }
}