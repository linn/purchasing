namespace Linn.Purchasing.Service.Modules.Reports
{
    using System;
    using System.Net.Mime;
    using System.Threading.Tasks;

    using Carter;
    using Carter.Request;
    using Carter.Response;

    using Linn.Common.Facade.Carter.Extensions;

    using Linn.Purchasing.Facade.Services;
    using Linn.Purchasing.Service.Extensions;
    using Linn.Purchasing.Service.Models;

    using Microsoft.AspNetCore.Http;

    public class SpendsReportModule : CarterModule
    {
        private readonly ISpendsReportFacadeService spendsReportFacadeService;

        public SpendsReportModule(ISpendsReportFacadeService spendsReportFacadeService)
        {
            this.spendsReportFacadeService = spendsReportFacadeService;
            this.Get("/purchasing/reports/spend-by-supplier", this.GetApp);
            this.Get("/purchasing/reports/spend-by-part", this.GetApp);
            this.Get("/purchasing/reports/spend-by-supplier/report", this.GetSpendBySupplierReport);
            this.Get("/purchasing/reports/spend-by-supplier/export", this.GetSpendBySupplierExport);
            this.Get("/purchasing/reports/spend-by-part/report", this.GetSpendByPartReport);
            this.Get("/purchasing/reports/spend-by-part/export", this.GetSpendByPartExport);
        }

        private async Task GetApp(HttpRequest req, HttpResponse res)
        {
            await res.Negotiate(new ViewResponse { ViewName = "Index.html" });
        }

        private async Task GetSpendByPartExport(HttpRequest req, HttpResponse res)
        {
            var supplierId = req.Query.As<int>("Id");

            var csv = this.spendsReportFacadeService.GetSpendByPartExport(
                supplierId,
                req.HttpContext.GetPrivileges());
            
            await res.FromCsv(csv, $"spendByPart_{supplierId}_{DateTime.Now.ToString("dd-MM-yyyy")}.csv");
        }

        private async Task GetSpendByPartReport(HttpRequest req, HttpResponse res)
        {
            var supplierId = req.Query.As<int>("Id");
            var results = this.spendsReportFacadeService.GetSpendByPartReport(
                supplierId,
                req.HttpContext.GetPrivileges());

            await res.Negotiate(results);
        }

        private async Task GetSpendBySupplierExport(HttpRequest req, HttpResponse res)
        {
            var vm = req.Query.As<string>("Vm");

            var csv = this.spendsReportFacadeService.GetSpendBySupplierExport(
                vm != null ? vm : string.Empty,
                req.HttpContext.GetPrivileges());

            var contentDisposition = new ContentDisposition
                                         {
                                             FileName = $"spendBySuppliers{DateTime.Now.ToString("dd-MM-yyyy")}.csv"
                                         };

            await res.FromCsv(csv, $"spendBySuppliers{DateTime.Now.ToString("dd-MM-yyyy")}.csv");
        }

        private async Task GetSpendBySupplierReport(HttpRequest req, HttpResponse res)
        {
            var vm = req.Query.As<string>("Vm");

            var results = this.spendsReportFacadeService.GetSpendBySupplierReport(
                vm != null ? vm : string.Empty,
                req.HttpContext.GetPrivileges());

            await res.Negotiate(results);
        }
    }
}
