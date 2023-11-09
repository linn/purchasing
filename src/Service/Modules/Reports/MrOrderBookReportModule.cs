namespace Linn.Purchasing.Service.Modules.Reports
{
    using System;
    using System.Threading.Tasks;

    using Carter;
    using Carter.Response;

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
            app.MapGet("/purchasing/reports/mr-order-book-export", this.GetExportStyleReport);
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

        private async Task GetExportStyleReport(
            HttpRequest req,
            HttpResponse res,
            int supplierId,
            IMrOrderBookReportFacadeService service)
        {
            var results = service.GetExportReport(supplierId);
            await res.Negotiate(results);
        }
    }
}
