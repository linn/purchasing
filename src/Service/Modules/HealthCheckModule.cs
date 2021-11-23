namespace Linn.Purchasing.Service.Modules
{
    using Carter;

    using Microsoft.AspNetCore.Http;

    public class HealthCheckModule : CarterModule
    {
        public HealthCheckModule()
        {
            this.Get("/healthcheck", async (req, res) => await res.WriteAsync("Ok"));
        }
    }
}
