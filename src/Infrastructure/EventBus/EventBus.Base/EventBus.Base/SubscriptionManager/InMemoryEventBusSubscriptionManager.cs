using RabbitListener.Application.Interface.EventBus;

namespace EventBus.Base.SubscriptionManager
{
    public class InMemoryEventBusSubscriptionManager : IEventBusSubscriptionManager
    {
        private readonly Dictionary<string, Type> _handlers;
        private readonly List<Type> _eventTypes;

        public event EventHandler<string> OnEventRemoved;

        public InMemoryEventBusSubscriptionManager()
        {
            _handlers = new Dictionary<string, Type>();
            _eventTypes = new List<Type>();
        }

        public bool IsEmpty => _handlers.Any() == false;

        public void AddSubscription<T, TH>() where T : IIntegrationEvent where TH : IIntegrationEventHandler<T>
        {
            var eventName = GetEventName<T>();

            AddSubscription(typeof(TH), eventName);

            if (_eventTypes.Contains(typeof(T)) == false)
            {
                _eventTypes.Add(typeof(T));
            }
        }

        public Type GetEventTypeByName(string eventName)
        {
            return _eventTypes.SingleOrDefault(s => s.Name == eventName);
        }

        public string GetEventName<T>()
        {
            var eventName = typeof(T).Name;

            return eventName;
        }

        public bool HasSubscriptionHandlerForEvent(string eventName)
        {
            return _handlers.ContainsKey(eventName);
        }

        public Type GetHandlerForEvent(string eventName)
        {
            return _handlers[eventName];
        }

        private void AddSubscription(Type eventHandlerType, string eventName)
        {
            if (HasSubscriptionHandlerForEvent(eventName) == false)
            {
                _handlers.Add(eventName, eventHandlerType);
            }
        }
    }
}
