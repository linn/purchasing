namespace Linn.Purchasing.Service.Modules.Reports
{
    using System.Threading.Tasks;

    using Carter;
    using Carter.Request;
    using Carter.Response;

    using Linn.Common.Facade.Carter.Extensions;
    using Linn.Purchasing.Facade.Services;
    using Linn.Purchasing.Service.Models;

    using Microsoft.AspNetCore.Http;

    public class WhatsDueInReportModule : CarterModule
    {
        private readonly IWhatsDueInReportFacadeService reportFacadeService;

        public WhatsDueInReportModule(IWhatsDueInReportFacadeService reportFacadeService)
        {
            this.reportFacadeService = reportFacadeService;
            this.Get("/purchasing/reports/whats-due-in", this.GetReport);
            this.Get("/purchasing/reports/whats-due-in/export", this.GetExport);

        }

        private async Task GetReport(HttpRequest req, HttpResponse res)
        {
            if (string.IsNullOrEmpty(req.Query.As<string>("fromDate")))
            {
                await res.Negotiate(new ViewResponse { ViewName = "Index.html" });
                return;
            }

            var results = this.reportFacadeService.GetReport(
                req.Query.As<string>("fromDate"),
                req.Query.As<string>("toDate"),
                req.Query.As<string>("orderBy"),
                req.Query.As<string>("vendorManager"),
                req.Query.As<int?>("supplier"));

            await res.Negotiate(results);
        }

        private async Task GetExport(HttpRequest req, HttpResponse res)
        {
            var csvResults = this.reportFacadeService.GetReportCsv(
                req.Query.As<string>("fromDate"),
                req.Query.As<string>("toDate"),
                req.Query.As<string>("orderBy"),
                req.Query.As<string>("vendorManager"),
                req.Query.As<int?>("supplier"));

            await res.FromCsv(csvResults, "whats_due_in.csv");
        }
    }
}
