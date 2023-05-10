namespace RabbitListener.Application.Interface.EventBus
{
    public interface IIntegrationEventHandler<T> where T : IIntegrationEvent
    {
        Task Handle(T @event);
    }
}