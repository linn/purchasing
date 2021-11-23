namespace Linn.Purchasing.Integration.Tests.Extensions
{
    using System.Net.Http;

    using Newtonsoft.Json;

    public static class HttpResponseMessageExtensions
    {
        public static T DeserializeBody<T>(this HttpResponseMessage res, JsonSerializerSettings settings = null)
        {
            return JsonConvert.DeserializeObject<T>(res.Content.ReadAsStringAsync().Result, settings);
        }
    }
}
