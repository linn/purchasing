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

    public class MrOrderBookReportModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/purchasing/reports/mr-order-book", this.GetReport);
            app.MapGet("/purchasing/reports/mr-order-book/export", this.GetExport);
        }

        private async Task GetReport(
            HttpRequest req,
            HttpResponse res,
            int? supplierId,
            IMrOrderBookReportFacadeService service)
        {
            if (!supplierId.HasValue)
            {
                await res.Negotiate(new ViewResponse { ViewName = "Index.html" });
            }
            else
            {
                var results = service.GetReport(supplierId.Value);
                await res.Negotiate(results);
            }
        }

        private async Task GetExport(
            HttpRequest req,
            HttpResponse res,
            IMrOrderBookReportFacadeService facadeService,
            int supplierId)
        {
            var csvResults = facadeService.GetExport(
                supplierId);
            var now = DateTime.Today;
            await res.FromCsv(csvResults, $"{supplierId}-order-book_{now.Day}-{now.Month}-{now.Year}.csv");
        }
    }
}
