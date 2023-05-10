namespace RabbitListener.Application.Interface.EventBus
{
    public interface IEventBusSubscriptionManager
    {
        bool IsEmpty { get; }

        event EventHandler<string> OnEventRemoved;

        void AddSubscription<T, TH>() where T : IIntegrationEvent where TH : IIntegrationEventHandler<T>;

        bool HasSubscriptionHandlerForEvent(string eventName);

        Type GetEventTypeByName(string eventName);

        Type GetHandlerForEvent(string eventName);

        string GetEventName<T>();
    }
}