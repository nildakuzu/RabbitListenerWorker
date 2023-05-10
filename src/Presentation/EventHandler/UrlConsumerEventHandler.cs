using RabbitListener.Application.Interface.EventBus;
using RabbitListener.Event;
using RabbitListener.HttpClientObj;
using RabbitListener.Log;

namespace RabbitListener.EventHandler
{
    public class UrlConsumerEventHandler : IIntegrationEventHandler<UrlConsumerEvent>
    {
        private readonly ILoger loger;
        private readonly IHttpClient httpClient;

        public UrlConsumerEventHandler(ILoger loger, IHttpClient httpClient)
        {
            this.loger = loger;
            this.httpClient = httpClient;
        }

        public async Task Handle(UrlConsumerEvent @event)
        {
            using (var client = httpClient.GetHttpClientObj())
            {
                var request = new HttpRequestMessage(HttpMethod.Head, @event.Url);
                int statusCode;

                try
                {
                    var response = await client.SendAsync(request);
                    statusCode = (int)response.StatusCode;
                }
                catch (Exception ex)
                {
                    statusCode = 500;
                }

                loger.Log(new Log.Log("RabbitListener", @event.Url, statusCode));

                await Task.FromResult(0);
            }
        }
    }
}
