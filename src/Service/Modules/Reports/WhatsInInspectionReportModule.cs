namespace Linn.Purchasing.Service.Modules.Reports
{
    using System.Threading.Tasks;

    using Carter;
    using Carter.Response;

    using Linn.Purchasing.Facade.Services;
    using Linn.Purchasing.Service.Models;

    using Microsoft.AspNetCore.Http;

    public class WhatsInInspectionReportModule : CarterModule
    {
        private readonly IWhatsInInspectionReportFacadeService facadeService;

        public WhatsInInspectionReportModule(IWhatsInInspectionReportFacadeService facadeService)
        {
            this.facadeService = facadeService;
            this.Get("/purchasing/reports/whats-in-inspection/report", this.GetReport);
            this.Get("/purchasing/reports/whats-in-inspection", this.GetApp);
        }

        private async Task GetApp(HttpRequest req, HttpResponse res)
        {
            await res.Negotiate(new ViewResponse { ViewName = "Index.html" });
        }

        private async Task GetReport(HttpRequest req, HttpResponse res)
        {
            var results = this.facadeService.GetReport();

            await res.Negotiate(results);
        }
    }
}
