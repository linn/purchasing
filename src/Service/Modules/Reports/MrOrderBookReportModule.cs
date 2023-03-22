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
    }
}
