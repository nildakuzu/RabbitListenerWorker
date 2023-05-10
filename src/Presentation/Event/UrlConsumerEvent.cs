using RabbitListener.Application.Model;

namespace RabbitListener.Event
{
    public class UrlConsumerEvent : IntegrationEvent
    {
        public string Url { get; set; }
    }
}
