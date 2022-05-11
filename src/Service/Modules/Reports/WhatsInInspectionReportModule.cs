namespace Linn.Purchasing.Service.Modules.Reports
{
    using System;
    using System.Threading.Tasks;

    using Carter;
    using Carter.Response;

    using Linn.Common.Facade.Carter.Extensions;
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
            app.MapGet("/purchasing/reports/whats-in-inspection/export", this.GetExport);
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
            bool showGoodStockQty,
            bool includePartsWithNoOrderNumber,
            bool showStockLocations,
            bool includeFailedStock,
            bool includeFinishedGoods,
            bool showBackOrdered,
            bool showOrders)
        {
            var results = facadeService.GetReport(
                showGoodStockQty,
                includePartsWithNoOrderNumber,
                showStockLocations,
                includeFailedStock,
                includeFinishedGoods,
                showBackOrdered,
                showOrders);

            await res.Negotiate(results);
        }

        private async Task GetExport(
            HttpRequest req,
            HttpResponse res,
            IWhatsInInspectionReportFacadeService facadeService,
            bool showGoodStockQty,
            bool includePartsWithNoOrderNumber,
            bool includeFailedStock,
            bool includeFinishedGoods)
        {
            var csvResults = facadeService.GetTopLevelExport(
                showGoodStockQty,
                includePartsWithNoOrderNumber,
                includeFailedStock,
                includeFinishedGoods);
            var now = DateTime.Today;
            await res.FromCsv(csvResults, $"whats_in_insp_{now.Day}-{now.Month}-{now.Year}.csv");
        }
    }
}
