namespace RabbitListener.Application.Interface.EventBus
{
    public interface IIntegrationEvent
    {
        public Guid Id { get; set; }

        public DateTime CreatedDateTime { get; set; }
    }
}
