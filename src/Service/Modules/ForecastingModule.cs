namespace Linn.Purchasing.Service.Modules
{
    using System.Threading.Tasks;

    using Carter;
    using Carter.Response;

    using Linn.Purchasing.Facade.Services;
    using Linn.Purchasing.Resources.RequestResources;
    using Linn.Purchasing.Service.Extensions;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class ForecastingModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/purchasing/forecasting/apply-percentage-change", this.ApplyPercentageChange);
        }

        private async Task ApplyPercentageChange(
            HttpRequest request,
            HttpResponse response,
            IForecastingFacadeService facadeService,
            ApplyForecastingPercentageChangeResource resource)
        {
            await response.Negotiate(facadeService.ApplyPercentageChange(
                resource.Change, resource.StartPeriod, resource.EndPeriod, request.HttpContext.GetPrivileges()));
        }
    }
}
