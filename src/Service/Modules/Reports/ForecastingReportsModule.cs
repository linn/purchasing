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
            app.MapGet("/purchasing/reports/weekly-forecast-orders/export", this.GetWeeklyExport);
            app.MapGet("/purchasing/reports/weekly-forecast-orders", this.GetApp);
        }

        private async Task GetApp(HttpRequest req, HttpResponse res)
        {
            await res.Negotiate(new ViewResponse { ViewName = "Index.html" });
        }

        private async Task GetWeeklyExport(
            HttpRequest req,
            HttpResponse res,
            IForecastingReportsFacadeService facadeService,
            int supplierId)
        {
            var csvResults = facadeService.GetWeeklyForecastExport(
               supplierId);
            var now = DateTime.Today;
            await res.FromCsv(csvResults, $"{supplierId}-weekly-forecast-orders_{now.Day}-{now.Month}-{now.Year}.csv");
        }
    }
}
