namespace Linn.Purchasing.Integration.Tests
{
    using System;
    using System.Net.Http;

    using Carter;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Json;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.DependencyInjection;

    public static class TestClient
    {
        public static HttpClient With<T>(Action<IServiceCollection> serviceConfiguration, params Func<RequestDelegate, RequestDelegate>[] middleWares) where T : ICarterModule
        {
            var server = new TestServer(
                new WebHostBuilder()
                    .ConfigureServices(
                        services => 
                            {
                                services.AddRouting();
                                services.Apply(serviceConfiguration);
                                services.AddCarter(configurator: c =>
                                    c.WithModule<T>());
                            })
                    .Configure(
                        app =>
                            {
                                app.UseRouting();
                                app.UseEndpoints(cfg => cfg.MapCarter());
                            }));

            return server.CreateClient();
        }
    }
}
