namespace Linn.Purchasing.Service.Modules.Reports
{
    using System.Threading.Tasks;

    using Carter;
    using Carter.Response;

    using Linn.Common.Facade.Carter.Extensions;
    using Linn.Purchasing.Facade.Services;
    using Linn.Purchasing.Service.Models;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class PrefSupReceiptsReportModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/purchasing/reports/pref-sup-receipts/report", this.GetReport);
            app.MapGet("/purchasing/reports/pref-sup-receipts", this.GetApp);
        }

        private async Task GetApp(HttpRequest req, HttpResponse res)
        {
            await res.Negotiate(new ViewResponse { ViewName = "Index.html" });
        }

        private async Task GetReport(
            HttpRequest req,
            HttpResponse res,
            IPrefSupReceiptsReportFacadeService reportFacadeService,
            string fromDate,
            string toDate)
        {
            var results = reportFacadeService.GetReport(fromDate, toDate);

            await res.Negotiate(results);
        }
    }
}
