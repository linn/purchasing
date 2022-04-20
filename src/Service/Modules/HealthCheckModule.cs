namespace Linn.Purchasing.Service.Modules
{
    using System.Threading.Tasks;

    using Carter;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class HealthCheckModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/healthcheck", this.Ok);
        }

        private async Task Ok(HttpRequest req, HttpResponse res)
        {
            await res.WriteAsync("Ok");
        }
    }
}
