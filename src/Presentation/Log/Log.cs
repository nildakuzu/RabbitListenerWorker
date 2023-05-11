namespace RabbitListener.Log
{
    public class Log
    {
        public Log(string serviceName, string url, int statusCode)
        {
            ServiceName = serviceName;
            Url = url;
            StatusCode = statusCode;
        }

        public string ServiceName { get; private set; }

        public string Url { get; private set; }

        public int StatusCode { get; private set; }
    }
}
