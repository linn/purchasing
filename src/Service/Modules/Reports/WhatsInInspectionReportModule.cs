namespace Linn.Purchasing.Service.Modules.Reports
{
    using System.Threading.Tasks;

    using Carter;
    using Carter.Response;

    using Linn.Purchasing.Facade.Services;
    using Linn.Purchasing.Service.Models;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class WhatsInInspectionReportModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/purchasing/reports/whats-in-inspection/report", this.GetReport);
            app.MapGet("/purchasing/reports/whats-in-inspection", this.GetApp);
        }

        private async Task GetApp(HttpRequest req, HttpResponse res)
        {
            await res.Negotiate(new ViewResponse { ViewName = "Index.html" });
        }

        private async Task GetReport(
            HttpRequest req,
            HttpResponse res,
            IWhatsInInspectionReportFacadeService facadeService,
            bool includePartsWithNoOrderNumber,
            bool showStockLocations,
            bool includeFailedStock,
            bool includeFinishedGoods,
            bool showBackOrdered)
        {
            var results = facadeService.GetReport(
                includePartsWithNoOrderNumber,
                showStockLocations,
                includeFailedStock,
                includeFinishedGoods,
                showBackOrdered);

            await res.Negotiate(results);
        }
    }
}
