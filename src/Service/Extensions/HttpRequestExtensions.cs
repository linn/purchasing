namespace Linn.Purchasing.Service.Extensions
{
    using System.IO;
    using System.Threading.Tasks;

    using Linn.Common.Serialization.Json;

    using Microsoft.AspNetCore.Http;

    public static class HttpRequestExtensions
    {
        public static async Task<T> Bind<T>(this HttpRequest req)
        {
            var reader = await new StreamReader(req.Body).ReadToEndAsync();
            var resource = new JsonSerializer()
                .Deserialize<T>(reader);
            return resource;
        }
    }
}
