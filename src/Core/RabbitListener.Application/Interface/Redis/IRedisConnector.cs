using StackExchange.Redis;

namespace RabbitListener.Application.Interface.Redis
{
    public interface IRedisConnector
    {
        public ConnectionMultiplexer Connect();

        public IDatabase GetDatabase();
    }
}