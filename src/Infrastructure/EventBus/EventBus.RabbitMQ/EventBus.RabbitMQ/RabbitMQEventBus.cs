using EventBus.Base;
using RabbitListener.Application.Model;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace EventBus.RabbitMQ
{
    public class RabbitMQEventBus : BaseEventBus
    {

        RabbitMQPersistentConnection persistentConnection;
        private readonly IConnectionFactory connectionFactory;
        private readonly IModel consumerChannel;

        public RabbitMQEventBus(IServiceProvider serviceProvider, EventBusConfig eventBusConfig) : base(serviceProvider, eventBusConfig)
        {
            connectionFactory = new ConnectionFactory();

            if (string.IsNullOrEmpty(eventBusConfig.HostName) == false)
            {
                connectionFactory = new ConnectionFactory()
                {
                    HostName = eventBusConfig.HostName,
                    Port = 5672
                };
            }

            persistentConnection = new RabbitMQPersistentConnection(connectionFactory, eventBusConfig.ConnectionRetryCount);
            consumerChannel = CreateConsumerChannel();
            EventBusSubscriptionManager.OnEventRemoved += EventBusSubscriptionManager_OnEventRemoved;
        }

        private void EventBusSubscriptionManager_OnEventRemoved(object sender, string eventName)
        {
            if (!persistentConnection.IsConnected)
            {
                persistentConnection.TryConnect();
            }

            consumerChannel.QueueUnbind(queue: EventBusConfig.CustomQueueName, exchange: EventBusConfig.DefaultTopicName, routingKey: eventName);

            if (EventBusSubscriptionManager.IsEmpty)
            {
                consumerChannel.Close();
            }
        }

        public override void Subscribe<E, EH>()
        {
            string queueName = EventBusConfig.CustomQueueName,
                eventName = typeof(E).Name;

            if (EventBusSubscriptionManager.HasSubscriptionHandlerForEvent(eventName) == false)
            {
                if (persistentConnection.IsConnected == false)
                {
                    persistentConnection.TryConnect();
                }

                consumerChannel.QueueDeclare(queue: queueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                consumerChannel.QueueBind(queue: queueName, exchange: EventBusConfig.DefaultTopicName, routingKey: eventName);
            }

            EventBusSubscriptionManager.AddSubscription<E, EH>();

            StartBasicConsume(queueName, eventName);
        }

        private IModel CreateConsumerChannel()
        {
            if (!persistentConnection.IsConnected)
            {
                persistentConnection.TryConnect();
            }

            var channel = persistentConnection.CreateModel();

            channel.ExchangeDeclare(exchange: EventBusConfig.DefaultTopicName, type: "direct");

            return channel;
        }

        private void StartBasicConsume(string queueName, string eventName)
        {
            if (consumerChannel != null)
            {
                var consumer = new EventingBasicConsumer(consumerChannel);

                consumer.Received += (sender, e) => Consumer_Received(sender, e, eventName)  ;

                consumerChannel.BasicConsume(queue: queueName, autoAck: false, consumerTag: EventBusConfig.SubscriberClientName, consumer: consumer);
            }
        }

        private async void Consumer_Received(object sender, BasicDeliverEventArgs e, string eventName)
        {
            var message = Encoding.UTF8.GetString(e.Body.Span);

            try
            {
                await ProcessEvent(eventName, message);
            }
            catch (Exception ex)
            {
                throw new Exception("There is a problem while processing event");
            }

            consumerChannel.BasicAck(e.DeliveryTag, multiple: false);
        }
    }
}
