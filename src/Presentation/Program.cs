using EventBus.RabbitMQ;
using Microsoft.Extensions.DependencyInjection;
using RabbitListener.Application.Interface.EventBus;
using RabbitListener.Application.Interface.Redis;
using RabbitListener.Application.Model;
using RabbitListener.Event;
using RabbitListener.EventHandler;
using RabbitListener.HttpClientObj;
using RabbitListener.Log;
using RabbitMQ.Client;
using Redis.Loger;
using RedisConnector;

class Program
{
    static void Main(string[] args)
    {
        ServiceProvider serviceProvider = new ServiceCollection()
            .AddSingleton<IEventBus>(sp =>
        {
            EventBusConfig config = new()
            {
                ConnectionRetryCount = 5,
                SubscriberClientName = "RabbitListenerWorker",
                Connection = new ConnectionFactory(),
                HostName = "c_rabbitmq",
                CustomQueueName = "urls"
            };

            return new RabbitMQEventBus(sp, config);

        }).AddTransient<ILoger, Loger>()
          .AddTransient<IRedisConnector, Connector>()
          .AddTransient<IRedisLoger, RedisLoger>()
          .AddTransient<IHttpClient, HttpClientObj>()
          .AddTransient<UrlConsumerEventHandler>().BuildServiceProvider();


        var eventBus = serviceProvider.GetService<IEventBus>();
        eventBus.Subscribe<UrlConsumerEvent, UrlConsumerEventHandler>();

        Console.Read();
    }
}
