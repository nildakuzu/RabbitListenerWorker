using RabbitListener.Application.Interface.Redis;
using StackExchange.Redis;

namespace RedisConnector
{
    public class Connector: IRedisConnector
    {
        public const string REDISCONSTR = "s_redis";

        public ConnectionMultiplexer Connect()
        {
            var redisConfigUrl = ConfigurationOptions.Parse(REDISCONSTR, true);
            redisConfigUrl.ResolveDns = true;

            return ConnectionMultiplexer.Connect(redisConfigUrl);
        }

        public IDatabase GetDatabase()
        {
            return Connect().GetDatabase();
        }
    }
}
