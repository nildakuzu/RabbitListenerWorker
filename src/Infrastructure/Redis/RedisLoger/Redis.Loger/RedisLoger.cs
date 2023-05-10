using RabbitListener.Application.Interface.Redis;
using StackExchange.Redis;

namespace Redis.Loger
{
    public class RedisLoger : IRedisLoger
    {
        private readonly IRedisConnector redisConnector;

        public RedisLoger(IRedisConnector redisConnector)
        {
            this.redisConnector = redisConnector;
        }

        public void Log(string log)
        {
            IDatabase db = redisConnector.GetDatabase();

            db.StringSet(string.Format(DateTime.Now.ToString()), log);
        }
    }
}
