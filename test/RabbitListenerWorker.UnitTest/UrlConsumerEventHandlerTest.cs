using Moq;
using RabbitListener.Event;
using RabbitListener.EventHandler;
using RabbitListener.HttpClientObj;
using RabbitListener.Log;
using Xunit;

namespace RabbitListenerWorker.UnitTest
{
    public class UrlConsumerEventHandlerTest
    {
        public Mock<ILoger> logerMock;
        public Mock<IHttpClient> httpClientMock;


        public UrlConsumerEventHandlerTest()
        {
            logerMock = new Mock<ILoger>();
            httpClientMock = new Mock<IHttpClient>();
        }

        [Fact]
        public async void if_head_request_is_done_response_should_be_logged()
        {
            var urlConsumerEventHandler = new UrlConsumerEventHandler(logerMock.Object, httpClientMock.Object);

            var httpClient = httpClientMock.Setup(s => s.GetHttpClientObj()).Returns(new HttpClient());

            await urlConsumerEventHandler.Handle(new UrlConsumerEvent());

            logerMock.Verify(s => s.Log(It.IsAny<Log>()), Times.Once);
        }

        [Fact]
        public async void if_head_request_url_is_invalid_it_should_be_throwed()
        {
            var urlConsumerEventHandler = new UrlConsumerEventHandler(logerMock.Object, httpClientMock.Object);

            var httpClient = httpClientMock.Setup(s => s.GetHttpClientObj()).Returns(new HttpClient());

            Assert.ThrowsAsync<Exception>(async () => await urlConsumerEventHandler.Handle(new UrlConsumerEvent() { Url = "https://googleee.com" }));
        }
    }
}
