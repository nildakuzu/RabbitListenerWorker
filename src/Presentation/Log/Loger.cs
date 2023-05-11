using Newtonsoft.Json;
using RabbitListener.Application.Interface.Redis;

namespace RabbitListener.Log
{
    public class Loger : ILoger
    {
        private readonly IRedisLoger redisLoger;

        public Loger(IRedisLoger redisLoger)
        {
            this.redisLoger = redisLoger;
        }

        public void Log(Log logModel)
        {
            var log = JsonConvert.SerializeObject(logModel);

            Console.WriteLine(log);

            redisLoger.Log(log);
        }
    }
}
