namespace RabbitListener.Application.Interface.EventBus
{
    public interface IEventBus
    {
        abstract void Subscribe<T, TH>() where T : IIntegrationEvent where TH : IIntegrationEventHandler<T>;
    }
}
