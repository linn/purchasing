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

    public class ForecastingReportsModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/purchasing/reports/monthly-forecast-orders/export", this.GetMonthlyExport);
            app.MapGet("/purchasing/reports/forecast-order-reports", this.GetApp);
        }

        private async Task GetApp(HttpRequest req, HttpResponse res)
        {
            await res.Negotiate(new ViewResponse { ViewName = "Index.html" });
        }

        private async Task GetMonthlyExport(
            HttpRequest req,
            HttpResponse res,
            IForecastingReportsFacadeService facadeService,
            int supplierId)
        {
            var csvResults = facadeService.GetMonthlyForecastExport(
               supplierId);
            var now = DateTime.Today;
            await res.FromCsv(csvResults, $"{supplierId}-monthly-forecast-orders_{now.Day}-{now.Month}-{now.Year}.csv");
        }
    }
}
