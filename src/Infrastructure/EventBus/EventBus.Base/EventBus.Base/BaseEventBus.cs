using EventBus.Base.SubscriptionManager;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitListener.Application.Interface.EventBus;
using RabbitListener.Application.Model;

namespace EventBus.Base
{
    public abstract class BaseEventBus : IEventBus
    {
        public readonly IServiceProvider _serviceProvider;
        public readonly IEventBusSubscriptionManager EventBusSubscriptionManager;
        public EventBusConfig EventBusConfig;

        public BaseEventBus(IServiceProvider serviceProvider, EventBusConfig eventBusConfig)
        {
            _serviceProvider = serviceProvider;
            EventBusSubscriptionManager = new InMemoryEventBusSubscriptionManager();
            EventBusConfig = eventBusConfig;
        }

        public async Task<bool> ProcessEvent(string eventName, string message)
        {
            var processed = false;

            if (EventBusSubscriptionManager.HasSubscriptionHandlerForEvent(eventName))
            {
                var subscriptionHandler = EventBusSubscriptionManager.GetHandlerForEvent(eventName);

                using (var scope = _serviceProvider.CreateScope())
                {
                    var handler = _serviceProvider.GetService(subscriptionHandler);

                    if (handler != null)
                    {
                        var eventType = EventBusSubscriptionManager.GetEventTypeByName($"{eventName}");

                        var integrationEvent = JsonConvert.DeserializeObject(message, eventType);

                        var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);

                        await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { integrationEvent });

                        processed = true;
                    }
                }
            }

            return processed;
        }

        public abstract void Subscribe<T, TH>()
            where T : IIntegrationEvent
            where TH : IIntegrationEventHandler<T>;

    }
}
