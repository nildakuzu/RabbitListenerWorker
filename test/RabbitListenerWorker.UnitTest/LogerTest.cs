using Moq;
using RabbitListener.Application.Interface.Redis;
using RabbitListener.Log;
using Xunit;

namespace RabbitListenerWorker.UnitTest
{
    public class LogerTest
    {
        public Mock<IRedisLoger> redisLogerMock;
        public LogerTest()
        {
            redisLogerMock = new Mock<IRedisLoger>();
        }

        [Fact]
        public void redis_hits_log_method()
        {
            new Loger(redisLogerMock.Object).Log(It.IsAny<Log>());

            redisLogerMock.Verify(s => s.Log(It.IsAny<string>()), Times.Once);

        }
    }
}
