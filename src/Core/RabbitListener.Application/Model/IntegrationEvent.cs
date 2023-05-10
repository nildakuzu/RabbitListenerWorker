using Newtonsoft.Json;
using RabbitListener.Application.Interface.EventBus;

namespace RabbitListener.Application.Model
{
    public class IntegrationEvent : IIntegrationEvent
    {
        [JsonProperty]
        public Guid Id { get; set; }

        [JsonProperty]
        public DateTime CreatedDateTime { get; set; }

        [JsonConstructor]
        public IntegrationEvent(Guid id, DateTime createdDateTime)
        {
            Id = id;
            CreatedDateTime = createdDateTime;
        }

        public IntegrationEvent()
        {
            Id = Guid.NewGuid();
            CreatedDateTime = DateTime.Now;
        }
    }
}
