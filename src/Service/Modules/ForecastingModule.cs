namespace Linn.Purchasing.Service.Modules
{
    using System.Threading.Tasks;

    using Carter;
    using Carter.Response;

    using Linn.Purchasing.Facade.Services;
    using Linn.Purchasing.Resources.RequestResources;
    using Linn.Purchasing.Service.Extensions;
    using Linn.Purchasing.Service.Models;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class ForecastingModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/purchasing/forecasting/apply-percentage-change", this.ApplyPercentageChange);
            app.MapGet("/purchasing/forecasting/apply-percentage-change", this.GetApp);
        }

        private async Task GetApp(HttpRequest req, HttpResponse res)
        {
            await res.Negotiate(new ViewResponse { ViewName = "Index.html" });
        }

        private async Task ApplyPercentageChange(
            HttpRequest request,
            HttpResponse response,
            IForecastingFacadeService reportsFacadeService,
            ApplyForecastingPercentageChangeResource resource)
        {
            await response.Negotiate(reportsFacadeService.ApplyPercentageChange(
                resource, request.HttpContext.GetPrivileges()));
        }
    }
}
