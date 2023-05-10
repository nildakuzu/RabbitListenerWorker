namespace RabbitListener.Application.Model
{
    public class EventBusConfig
    {
        public int ConnectionRetryCount { get; set; } = 3;

        public string DefaultTopicName { get; set; } = "urlstopic";

        public string SubscriberClientName { get; set; }

        public string HostName { get; set; }

        public object Connection { get; set; }

        public string CustomQueueName { get; set; }
    }
}
