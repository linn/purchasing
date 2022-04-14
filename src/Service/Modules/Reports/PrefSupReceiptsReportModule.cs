namespace Linn.Purchasing.Service.Modules.Reports
{
    using System.Threading.Tasks;

    using Carter;
    using Carter.Request;
    using Carter.Response;

    using Linn.Purchasing.Facade.Services;
    using Linn.Purchasing.Service.Models;

    using Microsoft.AspNetCore.Http;

    public class PrefSupReceiptsReportModule : CarterModule
    {
        private readonly IPrefSupReceiptsReportFacadeService reportFacadeService;

        public PrefSupReceiptsReportModule(IPrefSupReceiptsReportFacadeService reportFacadeService)
        {
            this.reportFacadeService = reportFacadeService;

            this.Get("/purchasing/reports/pref-sup-receipts/report", this.GetReport);
            this.Get("/purchasing/reports/pref-sup-receipts", this.GetApp);
        }

        private async Task GetApp(HttpRequest req, HttpResponse res)
        {
            await res.Negotiate(new ViewResponse { ViewName = "Index.html" });
        }

        private async Task GetReport(HttpRequest req, HttpResponse res)
        {
            var results = this.reportFacadeService.GetReport(
                req.Query.As<string>("fromDate"),
                req.Query.As<string>("toDate"));

            await res.Negotiate(results);
        }
    }
}
