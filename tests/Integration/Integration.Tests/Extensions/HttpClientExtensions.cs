namespace Linn.Purchasing.Integration.Tests.Extensions
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    public static class HttpClientExtensions
    {
        public static void Header(this HttpClient client, string header, string value)
        {
            client.DefaultRequestHeaders.Add(header, value);
        }

        public static void Accept(this HttpClient client, string accept)
        {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(accept));
        }

        public static Task<HttpResponseMessage> Get(this HttpClient client, string uri, Action<HttpClient> configurationAction)
        {
            configurationAction(client);

            return client.GetAsync(uri);
        }

        public static Task<HttpResponseMessage> Post(this HttpClient client, string uri, Action<HttpClient> configurationAction)
        {
            configurationAction(client);

            return client.PostAsync(uri, null);
        }

        public static Task<HttpResponseMessage> Put(this HttpClient client, string uri, Action<HttpClient> configurationAction)
        {
            configurationAction(client);

            return client.PutAsync(uri, null);
        }

        public static Task<HttpResponseMessage> Patch(this HttpClient client, string uri, Action<HttpClient> configurationAction)
        {
            configurationAction(client);

            var request = new HttpRequestMessage(new HttpMethod("PATCH"), uri);

            return client.SendAsync(request);
        }

        public static Task<HttpResponseMessage> Delete(this HttpClient client, string uri, Action<HttpClient> configurationAction)
        {
            configurationAction(client);

            return client.DeleteAsync(uri);
        }

        public static Task<HttpResponseMessage> Put<T>(this HttpClient client, string uri, T payload, Action<HttpClient> configurationAction)
        {
            configurationAction(client);

            var content = new StringContent(JsonConvert.SerializeObject(payload));

            return client.PutAsync(uri, content);
        }

        public static Task<HttpResponseMessage> Post<T>(
            this HttpClient client, 
            string uri, 
            T payload, 
            Action<HttpClient> configurationAction,
            string contentType = "application/json")
        {
            configurationAction(client);

            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, contentType);
            content.Headers.ContentType.CharSet = string.Empty;

            return client.PostAsync(uri, content);
        }
    }
}
