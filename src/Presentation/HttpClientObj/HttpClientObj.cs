namespace RabbitListener.HttpClientObj
{
    public class HttpClientObj : IHttpClient
    {
        public HttpClient GetHttpClientObj()
        {
            return new HttpClient();
        }
    }
}
